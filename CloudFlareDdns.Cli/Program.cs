using CloudFlareDdns.Cli.Services;
using CloudFlareDdns.SharedLogic.Interfaces;
using CommandLine;

namespace CloudFlareDdns.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GetIpOptions, UpdateOptions>(args)
                .MapResult(
                    (GetIpOptions opts) => Actions.GetIp(opts, new ChannelFactoryService().CreateChannelFactory<ICloudFlareDdnsCommsService>(opts)),
                    (UpdateOptions opts) => Actions.Update(opts, new ChannelFactoryService().CreateChannelFactory<ICloudFlareDdnsCommsService>(opts)),
                    errs => 1);
        }
    }
}
