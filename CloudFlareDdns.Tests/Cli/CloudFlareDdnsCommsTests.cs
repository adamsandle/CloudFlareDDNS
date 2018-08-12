using CloudFlareDdns.Cli;
using CloudFlareDdns.SharedLogic.Interfaces;
using Moq;
using NUnit.Framework;

namespace CloudFlareDdns.Tests.Cli
{
    [TestFixture]
    public class CloudFlareDdnsCommsTests
    {
        protected Mock<ICloudFlareDdnsCommsService> _commsService;
        protected Mock<IOutputService> _outputService;
        protected NetworkActions _networkActions;

        [SetUp]
        public void Setup()
        {
            _commsService = new Mock<ICloudFlareDdnsCommsService>();
            _outputService = new Mock<IOutputService>();
            _networkActions = new NetworkActions(_commsService.Object, _outputService.Object);
        }
    }
}