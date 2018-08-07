using YamlDotNet.Serialization;

namespace CloudFlareDdns.Service.Models
{
    public class UserConfig
    {
        public string Email { get; set; }
        [YamlMember(Alias = "api_key", ApplyNamingConventions = false)]
        public string ApiKey { get; set; }
        [YamlMember(Alias = "hosts_to_update", ApplyNamingConventions = false)]
        public HostToUpdate[] HostsToUpdate { get; set; }
    }

    public class HostToUpdate
    {
        public string Hostname { get; set; }
    }
}