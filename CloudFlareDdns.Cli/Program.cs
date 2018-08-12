using CloudFlareDdns.Cli.Services;
using CloudFlareDdns.SharedLogic.Interfaces;
using CommandLine;

namespace CloudFlareDdns.Cli
{
    class Program
    {
        private static NetworkActions _networkActions;
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GetIpOptions, UpdateOptions>(args)
                .MapResult(
                    (GetIpOptions opts) =>
                    {
                        Setup(opts);
                        return _networkActions.GetIp(opts);
                    },
                    (UpdateOptions opts) =>
                    {
                        Setup(opts);
                        return _networkActions.Update(opts);
                    },
                    errs => 1);
        }

        static void Setup(NetworkOptions opts)
        {
            var commsService = new ChannelFactoryService().CreateChannelFactory<ICloudFlareDdnsCommsService>(opts);
            var loggerService = new ConsoleOutputService();
            _networkActions = new NetworkActions(commsService, loggerService);
        }
    }
}
