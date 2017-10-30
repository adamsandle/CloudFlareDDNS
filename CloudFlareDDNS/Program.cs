using System.ServiceProcess;
namespace CloudFlareDDNS
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
                new CloudFlareDdnsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
