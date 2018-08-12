namespace CloudFlareDdns.SharedLogic.Models
{
    public class CloudFlareDdnsCommsServiceBaseResponse 
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class GetIpResponse : CloudFlareDdnsCommsServiceBaseResponse
    {
        public string IpAddress { get; set; }
    }

    public class UpdateResponse : CloudFlareDdnsCommsServiceBaseResponse
    {
        public string[] HostsUpdated { get; set; }
        public string[] HostsNotUpdated { get; set; }
    }
}
