using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Service
{
    public partial class CloudFlareDdnsService : ServiceBase
    {
        private static ServiceHost _host;
        private Timer _eventTimer;
        public static IpResponse IpResponse;
        private bool _isUpdating;
        public static UserConfig UserConfig;
        private readonly IOutputService _outputService;
        private readonly IHttpService _httpService;
        private readonly IUserConfigService _configService;
        private readonly CloudFlareApi _cloudFlareApi;
        public CloudFlareDdnsService(IOutputService outputService, IHttpService httpService, IUserConfigService configService, IServiceHostFactory serviceHostFactory)
        {
            _outputService = outputService;
            _httpService = httpService;
            _configService = configService;
            _host = serviceHostFactory.CreateServiceHost();
            _cloudFlareApi = new CloudFlareApi(_outputService, _httpService);

            InitializeComponent();
            _isUpdating = false;
        }

        protected override void OnStart(string[] args)
        {
            _outputService.WriteLine("CloudFlare DDNS service started");
            _configService.CreateFolder();
            UserConfig = _configService.GetUserConfig();
            if (UserConfig == null)
            {
                Environment.Exit(0);
            }
            _host.Open();
            _eventTimer = new Timer {Interval = 10000};
            _eventTimer.Elapsed += EventTimerTick;
            _eventTimer.Enabled = true;
        }

        private async void EventTimerTick(object sender, ElapsedEventArgs e)
        {
            if ((IpResponse == null && !_isUpdating || IpResponse != null && (DateTime.UtcNow - IpResponse.Updated).Minutes > 4) && !_isUpdating)
            {
                await UpdateDns(UserConfig.Hosts);
            }
        }

        private async Task<UpdateResponse> UpdateDns(string[] hosts, bool forceUpdate = false)
        {
            _isUpdating = true;
            var currentIp = await _httpService.GetPublicIp();
            var updateDns = false;
            if (currentIp != null && IpResponse?.Ip != currentIp.Ip)
            {
                _outputService.WriteLine("Ip updated:" + currentIp.Ip);
                updateDns = true;
            }
            IpResponse = currentIp;

            UpdateResponse result = null;
            if (updateDns || forceUpdate)
            {
                result = await _cloudFlareApi.UpdateDns(hosts);
            }
            _isUpdating = false;
            return result;
        }

        public GetIpResponse GetIp()
        {
            if (IpResponse != null)
            {
                return new GetIpResponse
                {
                    Success = true,
                    IpAddress = IpResponse.Ip
                };
            }

            return new GetIpResponse
            {
                Success = false,
                ErrorMessage = "Failed to get IP Address"
            };

        }

        public async Task<UpdateResponse> ForceUpdate(string[] hosts)
        {
            var result = await UpdateDns(hosts.Length > 0 ? hosts : UserConfig.Hosts, true);
            return result;
        }

        protected override void OnStop()
        {
            _host?.Close();
            _eventTimer.Enabled = false;
            _outputService.WriteLine("CloudFlareDDNS service stopped");
        }
    }
}
