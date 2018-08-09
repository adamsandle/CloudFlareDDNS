using System.Collections.Generic;
using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Service
{
    public class CloudFlareDdnsCommsService : ICloudFlareDdnsCommsService
    {
        public string GetIp()
        {
            return Program._service.GetIp();
        }

        public UpdateResponse ForceUpdate(string[] hosts)
        {
            var goo =  Program._service.ForceUpdate(hosts).Result;
            return goo;
        }
    }
}