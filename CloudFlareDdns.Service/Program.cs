using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace CloudFlareDdns.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                _service = new CloudFlareDdnsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        public static CloudFlareDdnsService _service;


    }
}
