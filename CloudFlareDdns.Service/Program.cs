using System;
using System.IO;
using System.ServiceProcess;
using CloudFlareDdns.Service.Services;

namespace CloudFlareDdns.Service
{
    static class Program
    {
        public static CloudFlareDdnsService _service;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var fileOutputService = new FileOutputService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CloudFlareDDNS"));
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                _service = new CloudFlareDdnsService(fileOutputService, new HttpService(fileOutputService), new UserConfigService(fileOutputService, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CloudFlareDDNS")), new ServiceHostFactory())
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
