using System;
using System.ServiceModel;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Service.Services
{
    public class ServiceHostFactory : IServiceHostFactory
    {
        public ServiceHost CreateServiceHost()
        {
            var service = new ServiceHost(
                typeof(CloudFlareDdnsCommsService),
                new Uri[]
                {
                    new Uri("http://0.0.0.0:8320"),
                    new Uri("net.pipe://0.0.0.0")
                });
            service.AddServiceEndpoint(typeof(ICloudFlareDdnsCommsService),
                new BasicHttpBinding(),
                "Reverse");

            service.AddServiceEndpoint(typeof(ICloudFlareDdnsCommsService),
                new NetNamedPipeBinding(),
                "PipeReverse");
            return service;
        }
    }
}