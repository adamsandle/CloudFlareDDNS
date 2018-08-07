namespace CloudFlareDdns.Service.Models.Response
{
    public class CloudFlareBaseResponse
    {
        public bool Success { get; set; }
        public CloudFlareMessageResponse[] Errors { get; set; }
        public CloudFlareMessageResponse[] Messages { get; set; }
    }

    public class CloudFlareMessageResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}