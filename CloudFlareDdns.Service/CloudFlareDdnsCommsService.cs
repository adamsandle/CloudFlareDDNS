using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Service
{
    public class CloudFlareDdnsCommsService : ICloudFlareDdnsCommsService
    {
        public GetIpResponse GetIp()
        {
            return Program._service.GetIp();
        }

        public UpdateResponse ForceUpdate(string[] hosts)
        {
            return  Program._service.ForceUpdate(hosts).Result;
        }
    }
}