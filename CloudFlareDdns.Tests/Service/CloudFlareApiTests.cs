using System;
using System.Threading.Tasks;
using CloudFlareDdns.Service;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.SharedLogic.Interfaces;
using Moq;
using NUnit.Framework;

namespace CloudFlareDdns.Tests.Service
{
    [TestFixture]
    public class CloudFlareApiTests
    {
        protected Mock<IOutputService> _outputService;
        protected Mock<IHttpService> _httpService;
        protected CloudFlareApi _cloudFlareApi;

        [SetUp]
        public void Setup()
        {
            _outputService = new Mock<IOutputService>();
            _httpService = new Mock<IHttpService>();
            _cloudFlareApi = new CloudFlareApi(_outputService.Object, _httpService.Object);
            Foo.SetupHttpService(_httpService);
            
        }

        [Test]
        public async Task UpdateDnsUpdatesCorrectly()
        {
            var result = await _cloudFlareApi.UpdateDns(new []{"domain1.com"}, "1.2.3.4");

            Assert.True(result.Success);
            Assert.AreEqual(1, result.HostsUpdated.Length);
            Assert.AreEqual(0, result.HostsNotUpdated.Length);
        }

        [Test]
        public async Task UpdateDnsWithFailingUpdateWorkCorrectly()
        {
            _httpService.Setup(x => x.UpdateRecord("zoneId", "recordId", It.IsAny<CloudFlareUpdateDnsRecordRequest>())).ReturnsAsync(new CloudFlareBaseResponse
            {
                Success = false
            });

            var result = await _cloudFlareApi.UpdateDns(new[] { "foo.bar.com" }, "1.2.3.4");

            Assert.False(result.Success);
            Assert.AreEqual(0, result.HostsUpdated.Length);
            Assert.AreEqual(1, result.HostsNotUpdated.Length);
        }

        [Test]
        public async Task UpdateDnsOutputsCorrectly()
        {
            await _cloudFlareApi.UpdateDns(new[] { "domain1.com" }, "1.2.3.4");

            _outputService.Verify(x => x.WriteLine("domain1.com Successfully Updated"), Times.Once);
            _outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public async Task UpdateDnsCatchesExceptionCorrectly()
        {
            var exception = new Exception("Exception");
            _httpService.Setup(x => x.UpdateRecord(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CloudFlareUpdateDnsRecordRequest>()))
                .ThrowsAsync(exception);

            var result = await _cloudFlareApi.UpdateDns(new[] { "domain1.com" }, "1.2.3.4");

            _outputService.Verify(x => x.WriteLine(exception), Times.Once);
            _outputService.Verify(x => x.WriteLine(It.IsAny<Exception>()), Times.Once());
            Assert.False(result.Success);
            Assert.AreEqual(0, result.HostsUpdated.Length);
            Assert.AreEqual(1, result.HostsNotUpdated.Length);
        }
    }

    public static class Foo
    {
        public static void SetupHttpService(Mock<IHttpService> _httpService)
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