using CloudFlareDdns.Service;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.SharedLogic.Interfaces;
using Moq;
using NUnit.Framework;

namespace CloudFlareDdns.Tests.Service
{
    [TestFixture]
    public class CloudFlareDdnsServiceTests
    {
        protected Mock<IOutputService> _outputService;
        protected Mock<IHttpService> _httpService;
        protected Mock<IUserConfigService> _configService;
        protected Mock<IServiceHostFactory> _serviceHostFactory;
        protected CloudFlareDdnsService _service;

        [SetUp]
        public void Setup()
        {
            _outputService = new Mock<IOutputService>();
            _httpService = new Mock<IHttpService>();
            _configService = new Mock<IUserConfigService>();
            _serviceHostFactory = new Mock<IServiceHostFactory>();
            _service = new CloudFlareDdnsService(_outputService.Object, _httpService.Object, _configService.Object,
                _serviceHostFactory.Object);
        }

        [Test]
        public void OnStartRunsCorrectly()
        {
//            _service.ons
        }
    }
}