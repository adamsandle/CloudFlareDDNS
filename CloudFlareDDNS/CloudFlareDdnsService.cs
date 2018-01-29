using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using CloudFlareDDNS.Models;

namespace CloudFlareDDNS
{
    public partial class CloudFlareDdnsService : ServiceBase
    {
        private Timer _eventTimer;
        private IpResponse _ipResponse;
        private bool _isUpdating;
        public CloudFlareDdnsService()
        {
            InitializeComponent();
            _isUpdating = false;
        }

        protected override void OnStart(string[] args)
        {
            _eventTimer = new Timer();
            _eventTimer.Interval = 10000;
            _eventTimer.Elapsed += EventTimerTick;
            _eventTimer.Enabled = true;
            Logger.WriteLog("Test windows service started");
            string folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CloudFlareDDNS");
            Directory.CreateDirectory(folderName);
        }

        private async void EventTimerTick(object sender, ElapsedEventArgs e)
        {
            if ((_ipResponse == null || (DateTime.UtcNow - _ipResponse.Updated).Minutes > 4) && !_isUpdating)
            {
                _isUpdating = true;
                _ipResponse = await Http.GetPublicIp();
                Logger.WriteLog("Ip updated:" + _ipResponse.Ip);
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
