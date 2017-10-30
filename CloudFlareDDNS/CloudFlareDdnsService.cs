using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace CloudFlareDDNS
{
    public partial class CloudFlareDdnsService : ServiceBase
    {
        private Timer _eventTimer;
        public CloudFlareDdnsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _eventTimer = new Timer();
            _eventTimer.Interval = 10000;
            _eventTimer.Elapsed += EventTimerTick;
            _eventTimer.Enabled = true;
            Library.WriteErrorLog("Test windows service started");
            string folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CloudFlareDDNS");
            Directory.CreateDirectory(folderName);
        }

        private void EventTimerTick(object sender, ElapsedEventArgs e)
        {
            Library.GetPublicIp();
            
        }

        protected override void OnStop()
        {
            _eventTimer.Enabled = false;
            Library.WriteErrorLog("Test service stopped");
        }
    }
}
