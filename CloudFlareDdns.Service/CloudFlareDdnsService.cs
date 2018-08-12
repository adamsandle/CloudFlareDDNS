using System;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.Service.Utils;
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
        public CloudFlareDdnsService()
        {
            InitializeComponent();
            _isUpdating = false;
        }

        protected override void OnStart(string[] args)
        {
            Logger.WriteLog("CloudFlare DDNS service started");
            Directory.CreateDirectory(Config.Folder);
            UserConfig = Config.GetUserConfig();
            if (UserConfig == null)
            {
                Environment.Exit(0);
            }
            SetUpServer();
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
            var currentIp = await Http.GetPublicIp();
            var updateDns = false;
            if (currentIp != null && IpResponse?.Ip != currentIp.Ip)
            {
                Logger.WriteLog("Ip updated:" + currentIp.Ip);
                updateDns = true;
            }
            IpResponse = currentIp;

            UpdateResponse result = null;
            if (updateDns || forceUpdate)
            {
                result = await CloudFlareApi.UpdateDns(hosts);
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
            Logger.WriteLog("Test service stopped");
        }

        private static void SetUpServer()
        {
            _host = new ServiceHost(
                typeof(CloudFlareDdnsCommsService),
                new Uri[]
                {
                    new Uri("http://0.0.0.0:8320"),
                    new Uri("net.pipe://0.0.0.0")
                });
            _host.AddServiceEndpoint(typeof(ICloudFlareDdnsCommsService),
                new BasicHttpBinding(),
                "Reverse");

            _host.AddServiceEndpoint(typeof(ICloudFlareDdnsCommsService),
                new NetNamedPipeBinding(),
                "PipeReverse");

            _host.Open();
        }
    }
}
