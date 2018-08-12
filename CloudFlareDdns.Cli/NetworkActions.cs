using System;
using System.Linq;
using System.ServiceModel;
using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Cli
{
    public class NetworkActions
    {
        private readonly ICloudFlareDdnsCommsService _commsService;
        private readonly IOutputService _outputService;
        public NetworkActions(ICloudFlareDdnsCommsService commsService, IOutputService outputService)
        {
            _commsService = commsService;
            _outputService = outputService;
        }

        public int GetIp(GetIpOptions opts)
        {
            return Exceute(() =>
            {
                var result = _commsService.GetIp();
                if (result.Success)
                {
                    _outputService.WriteLine(result.IpAddress);
                }

                return result;
            });
        }

        public int Update(UpdateOptions opts)
        {
            return Exceute(() =>
            {
                var result = _commsService.ForceUpdate(opts.Hosts.ToArray());

                foreach (var host in result.HostsUpdated)
                {
                    _outputService.WriteLine("Host updated: " + host);
                }

                foreach (var host in result.HostsNotUpdated)
                {
                    _outputService.WriteLine("Host not updated: " + host);
                }

                return result;
            });
        }

        private int Exceute(Func<CloudFlareDdnsCommsServiceBaseResponse> method)
        {
            try
            {
                var result = method();
                if (result.Success)
                {
                    return 0;
                }

                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    _outputService.WriteLine(result.ErrorMessage);
                }
            }
            catch (EndpointNotFoundException)
            {
                _outputService.WriteLine("Could not connect to the server");
            }
            catch (Exception e)
            {
                _outputService.WriteLine(e);
            }

            return 1;
        }
    }
}