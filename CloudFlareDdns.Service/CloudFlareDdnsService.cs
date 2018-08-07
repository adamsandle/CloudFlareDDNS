using System;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.SharedLogic.Interfaces;

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
                _isUpdating = true;
                var currentIp = await Http.GetPublicIp();
                var updateDns = false;
                if (currentIp != null && IpResponse?.Ip != currentIp.Ip)
                {
                    Logger.WriteLog("Ip updated:" + currentIp.Ip);
                    updateDns = true;
                }
                IpResponse = currentIp;

                if (updateDns)
                {
                    await CloudFlareApi.UpdateDns();
                }
                _isUpdating = false;
            }
        }

        public string GetIp()
        {
            return IpResponse.Ip;
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
                    new Uri("http://localhost:8320"),
                    new Uri("net.pipe://localhost")
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
