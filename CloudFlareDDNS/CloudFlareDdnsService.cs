using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using CloudFlareDDNS.Models;
using CloudFlareDDNS.Models.Response;

namespace CloudFlareDDNS
{
    public partial class CloudFlareDdnsService : ServiceBase
    {
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

        protected override void OnStop()
        {
            _eventTimer.Enabled = false;
            Logger.WriteLog("Test service stopped");
        }
    }
}
