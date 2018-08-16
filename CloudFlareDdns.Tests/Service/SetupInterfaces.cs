using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using Moq;

namespace CloudFlareDdns.Tests.Service
{
    public static class SetupInterfaces
    {
        public static void Setup(Mock<IHttpService> _httpService)
        {
            _httpService.Setup(x => x.GetZones()).ReturnsAsync(new CloudFlareZonesResponse
            {
                Result = new[]
                {
                    new CloudFlareZoneResultResponse
                    {
                        Id = "domain1",
                        Name = "domain1.com"
                    },
                    new CloudFlareZoneResultResponse
                    {
                        Id = "domain2",
                        Name = "domain2.com"
                    }
                },
                Success = true
            });

            _httpService.Setup(x => x.GetRecords("domain1")).ReturnsAsync(new CloudFlareDnsRecordsResponse
            {
                Result = new[]
                {
                    new CloudFlareDnsRecordsResultResponse
                    {
                        Name = "domain1.com",
                        Zone_Id = "domain1",
                        Id = "domain1"
                    }
                },
                Success = true
            });

            _httpService.Setup(x => x.GetRecords("domain2")).ReturnsAsync(new CloudFlareDnsRecordsResponse
            {
                Result = new[]
                {
                    new CloudFlareDnsRecordsResultResponse
                    {
                        Name = "domain2.com",
                        Zone_Id = "domain2",
                        Id = "domain2"
                    }
                },
                Success = true
            });

            _httpService.Setup(x => x.GetRecordDetails("domain1", "domain1")).ReturnsAsync(
                new CloudFlareDnsRecordResponse
                {
                    Result = new CloudFlareDnsRecordResultResponse
                    {
                        Name = "domain1.com",
                        Zone_Id = "domain1",
                        Id = "domain1",
                        Content = "content",
                        Proxied = true,
                        Ttl = 0,
                        Type = "A"
                    },
                    Success = true
                });

            _httpService.Setup(x => x.GetRecordDetails("domain2", "domain2")).ReturnsAsync(
                new CloudFlareDnsRecordResponse
                {
                    Result = new CloudFlareDnsRecordResultResponse
                    {
                        Name = "domain2.com",
                        Zone_Id = "domain2",
                        Id = "domain2",
                        Content = "content",
                        Proxied = true,
                        Ttl = 0,
                        Type = "A"
                    },
                    Success = true
                });

            _httpService.Setup(x => x.UpdateRecord(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CloudFlareUpdateDnsRecordRequest>())).ReturnsAsync(new CloudFlareBaseResponse
            {
                Success = true
            });
        }
    }
}