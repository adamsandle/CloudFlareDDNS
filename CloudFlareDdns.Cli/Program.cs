using System;
using System.ServiceModel;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ICloudFlareDdnsCommsService> httpFactory =
                new ChannelFactory<ICloudFlareDdnsCommsService>(
                    new BasicHttpBinding(),
                    new EndpointAddress(
                        "http://localhost:8320/Reverse"));

            ChannelFactory<ICloudFlareDdnsCommsService> pipeFactory =
                new ChannelFactory<ICloudFlareDdnsCommsService>(
                    new NetNamedPipeBinding(),
                    new EndpointAddress(
                        "net.pipe://localhost/PipeReverse"));

            ICloudFlareDdnsCommsService httpProxy =
                httpFactory.CreateChannel();

            ICloudFlareDdnsCommsService pipeProxy =
                pipeFactory.CreateChannel();

            while (true)
            {
                string str = System.Console.ReadLine();
                Console.WriteLine("pipe: " +
                                  pipeProxy.GetIp());
//                WriteLine("http: " +
//                                  httpProxy.ReverseString(str));
//                WriteLine("pipe: " +
//                                  pipeProxy.ReverseString(str));
            }
        }
    }
}
