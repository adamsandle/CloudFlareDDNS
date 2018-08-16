using System;
using System.Threading.Tasks;
using CloudFlareDdns.Service;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using Moq;
using NUnit.Framework;

namespace CloudFlareDdns.Tests.Service
{
    [TestFixture]
    public class ForceUpdateTests : CloudFlareDdnsServiceTests
    {
        [SetUp]
        public void Setup()
        {
            Foo.SetupHttpService(_httpService);
            _configService.Setup(x => x.GetUserConfig()).Returns(new UserConfig
            {
                Hosts = new[] {"domain1.com", "domain2.com"}
            });
        }

        [Test]
        public async Task ForceUpdateWorksCorrectlyWhenUsingConfig()
        {
            _service.ReloadConfig();
            var result = await _service.ForceUpdate(new string[]{});

            Assert.True(result.Success);
            Assert.AreEqual(2, result.HostsUpdated.Length);
            Assert.AreEqual(0, result.HostsNotUpdated.Length);
        }

        [Test]
        public async Task ForceUpdateWorkCorrectlyWhenSpecifyingHosts()
        {
            _service.ReloadConfig();
            var result = await _service.ForceUpdate(new [] {"domain1.com"});

            Assert.True(result.Success);
            Assert.AreEqual(1, result.HostsUpdated.Length);
            Assert.AreEqual(0, result.HostsNotUpdated.Length);
        }

        [Test]
        public async Task ForceUpdateFailsWhenSpecifyingHosts()
        {
            _service.ReloadConfig();
            var result = await _service.ForceUpdate(new[] { "domain1.com", "foo.com" });

            Assert.False(result.Success);
            Assert.AreEqual(1, result.HostsUpdated.Length);
            Assert.AreEqual(1, result.HostsNotUpdated.Length);
        }
    }
}