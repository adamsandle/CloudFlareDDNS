using System.Collections.Generic;
using CommandLine;

namespace CloudFlareDdns.Cli
{
    public class NetworkOptions
    {
        [Option(Default = "localhost", HelpText = "The hostname of the remote machine.")]
        public string IpAddress { get; set; }

        [Option(Default = 8320, HelpText = "The port to connect to.")]
        public int Port { get; set; }
    }

    [Verb("update", HelpText = "Update DNS records")]
    public class UpdateOptions : NetworkOptions
    {
        [Option('h',"host")] public IEnumerable<string> Hosts { get; set; }
    }

    [Verb("getip", HelpText = "Get the IP of the remote machine")]
    public class GetIpOptions : NetworkOptions
    {
    }
}