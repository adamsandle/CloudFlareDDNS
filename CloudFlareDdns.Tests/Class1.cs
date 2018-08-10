using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CloudFlareDdns.Tests
{
    [TestFixture]
    public class CalculatorTest
    {
        static Mock<ICloudFlareDdnsCommsService> myInterfaceMock;
        private static ServiceHost mockServiceHost;

        [SetUp]
        public void Setup()
        {
            Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add<ServiceContractAttribute>();
            myInterfaceMock = new Mock<ICloudFlareDdnsCommsService>();
            myInterfaceMock.Setup(x => x.ForceUpdate(new[] {"success"})).Returns(new UpdateResponse {Success = true, HostsUpdated = new []{"success"}, HostsNotUpdated = new string[]{}});
            mockServiceHost = MockServiceHostFactory.GenerateMockServiceHost(myInterfaceMock.Object, new Uri("http://localhost:8320"), "Reverse");
            mockServiceHost.Open();
        }

        [TearDown]
        public static void TearDown()
        {
            mockServiceHost.Close();
        }
        [Test]
        public static void ShouldAddTwoNumbers()
        {
            Cli.Program.Main(new []{"update", "-h", "success"});
            mockServiceHost.Close();
            myInterfaceMock.Verify(mock => mock.ForceUpdate(new []{"success"}), Times.Once());

        }

        [Test]
        public static void TestShouldFail()
        {
            Assert.True(false);
        }

    }

    public static class MockServiceHostFactory
    {
        public static ServiceHost GenerateMockServiceHost<TMock>(TMock mock, Uri baseAddress, string endpointAddress)
        {
            var serviceHost = new ServiceHost(mock, new[] { baseAddress });

            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
            serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>().InstanceContextMode = InstanceContextMode.Single;

            serviceHost.AddServiceEndpoint(typeof(TMock), new BasicHttpBinding(), endpointAddress);

            return serviceHost;
        }
    }
}
