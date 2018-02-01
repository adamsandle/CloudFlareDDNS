namespace CloudFlareDDNS.Models.Response
{
    public class CloudFlareZonesResponse
    {
        public bool Success { get; set; }
        public CloudFlareZoneResultResponse[] Result { get; set; }
    }

    public class CloudFlareZoneResultResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}