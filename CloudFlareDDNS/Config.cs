using System;
using System.IO;
using CloudFlareDDNS.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CloudFlareDDNS
{
    public static class Config
    {
        public static readonly string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CloudFlareDDNS");

        public static readonly string LogFile = Path.Combine(Folder, "Logfile.txt");
        private static readonly string UserConfigFile = Path.Combine(Folder, "Config.yml");

        public const string CloudFlareBaseUrl = "https://api.cloudflare.com/client/v4/";
        public const string IpUrl = "https://api.ipify.org?format=json";

        public static UserConfig GetUserConfig()
        {
            try
            {
                if (File.Exists(UserConfigFile))
                {
                    string input = File.ReadAllText(UserConfigFile);

                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(new CamelCaseNamingConvention())
                        .Build();

                    var userConfig = deserializer.Deserialize<UserConfig>(input);
                    return userConfig;
                }
                File.WriteAllText(UserConfigFile, UserConfigFileTemplate);
                Logger.WriteLog("Sample config saved");
                return null;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
                return null;
            }
        }

        private static readonly string UserConfigFileTemplate = @"---
email:		test@mail.com
api_key:	123-456-789-0
hosts_to_update:
    - hostname:   test.domain.com
    - hostname:   mail.domain.com";
    }
}