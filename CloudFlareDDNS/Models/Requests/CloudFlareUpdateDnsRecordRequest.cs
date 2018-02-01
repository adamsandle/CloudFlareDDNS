namespace CloudFlareDDNS.Models.Requests
{
    public class CloudFlareUpdateDnsRecordRequest
    {
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }
}