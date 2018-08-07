using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Service
{
    public class CloudFlareDdnsCommsService : ICloudFlareDdnsCommsService
    {
        public string GetIp()
        {
            return Program._service.GetIp();
        }
    }
}