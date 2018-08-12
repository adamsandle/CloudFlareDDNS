using System;
using CloudFlareDdns.Cli;
using CloudFlareDdns.SharedLogic.Models;
using Moq;
using NUnit.Framework;

namespace CloudFlareDdns.Tests.Cli
{
    [TestFixture]
    public class GetIpTests : CloudFlareDdnsCommsTests
    {
        [Test]
        public void GetIpWorksCorrectly()
        {
            _commsService.Setup(x => x.GetIp()).Returns(new GetIpResponse
            {
                Success = true,
                IpAddress = "1.2.3.4"
            });

            var result = _networkActions.GetIp(new GetIpOptions());

            _commsService.Verify(x => x.GetIp(), Times.Once);
            _outputService.Verify(x => x.WriteLine("1.2.3.4"), Times.Once);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetIpErrorsCorrectly()
        {
            var error = new Exception("Error");

            _commsService.Setup(x => x.GetIp()).Throws(error);

            var result = _networkActions.GetIp(new GetIpOptions());

            _outputService.Verify(x => x.WriteLine(error));
            Assert.AreEqual(1, result);
        }
    }
}
