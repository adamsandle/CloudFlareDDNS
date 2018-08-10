using System;
using System.Linq;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Cli
{
    public static class Actions
    {
        public static int GetIp(GetIpOptions opts, ICloudFlareDdnsCommsService commsService)
        {
            Console.WriteLine(commsService.GetIp());
            return 0;
        }

        public static int Update(UpdateOptions opts, ICloudFlareDdnsCommsService commsService)
        {
            var result = commsService.ForceUpdate(opts.Hosts.ToArray());

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
    }
}