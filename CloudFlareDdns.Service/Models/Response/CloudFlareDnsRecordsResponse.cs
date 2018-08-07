namespace CloudFlareDdns.Service.Models.Response
{
    public class CloudFlareDnsRecordsResponse : CloudFlareBaseResponse
    {
        public CloudFlareDnsRecordsResultResponse[] Result { get; set; }
    }
    
    public class CloudFlareDnsRecordsResultResponse
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Zone_Id { get; set; }
    }
}