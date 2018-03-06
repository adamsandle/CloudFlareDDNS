namespace CloudFlareDDNS.Models.Requests
{
    public class CloudFlareUpdateDnsRecordRequest : BaseRequest
    {
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public bool proxied { get; set; }
        public int ttl { get; set; }
    }
}