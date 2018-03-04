namespace CloudFlareDDNS.Models.Response
{
    public class CloudFlareDnsRecordResponse : CloudFlareBaseResponse
    {
        public CloudFlareDnsRecordResultResponse Result { get; set; }
    }

    public class CloudFlareDnsRecordResultResponse
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool Proxied { get; set; }
        public int Ttl { get; set; }
        public string Zone_Id { get; set; }
    }
}