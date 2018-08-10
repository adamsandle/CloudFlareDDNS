using System.ServiceModel;
using CloudFlareDdns.Cli.Interfaces;

namespace CloudFlareDdns.Cli.Services
{
    public class ChannelFactoryService : IChannelFactory
    {
        public T CreateChannelFactory<T>(NetworkOptions opts)
        {
            return new ChannelFactory<T>(
                new BasicHttpBinding(),
                new EndpointAddress(
                    "http://" + opts.IpAddress + ":" + opts.Port + "/Reverse")).CreateChannel();
        }
    }
}