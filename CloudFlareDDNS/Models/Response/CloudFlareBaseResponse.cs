namespace CloudFlareDDNS.Models.Response
{
    public class CloudFlareBaseResponse
    {
        public bool Success { get; set; }
        public CloudFlareErrorResponse[] Errors { get; set; }
    }

    public class CloudFlareErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}