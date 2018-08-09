using System.Threading.Tasks;
using CommandLine;

namespace CloudFlareDdns.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GetIpOptions, UpdateOptions>(args)
                .MapResult(
                    (GetIpOptions opts) => new Proxy(opts).GetIp(), 
                    (UpdateOptions opts) => new Proxy(opts).Update(),
                    errs => 1);
        }
    }
}
