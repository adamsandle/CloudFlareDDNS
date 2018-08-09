using System;
using System.Linq;
using System.ServiceModel;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Cli
{
    public class Proxy
    {
        private ICloudFlareDdnsCommsService httpProxy;
        private NetworkOptions _opts;
        public Proxy(NetworkOptions opts)
        {
            _opts = opts;
            httpProxy = new ChannelFactory<ICloudFlareDdnsCommsService>(
                new BasicHttpBinding(),
                new EndpointAddress(
                    "http://" + opts.IpAddress + ":" + opts.Port + "/Reverse")).CreateChannel();
        }

        public int Update()
        {
            var opts = (UpdateOptions) _opts;
            var result = httpProxy.ForceUpdate(opts.Hosts.ToArray());

            foreach (var host in result.HostsUpdated)
            {
                Console.WriteLine("Host updated: " + host);
            }

            foreach (var host in result.HostsNotUpdated)
            {
                Console.WriteLine("Host not updated: " + host);
            }

            if (result.Success)
            {
                return 0;
            }

            return 1;
        }

        public int GetIp()
        {
            Console.WriteLine(httpProxy.GetIp());
            return 0;
        }
    }
}