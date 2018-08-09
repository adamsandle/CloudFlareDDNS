using System;
using System.IO;
using System.Xml.Serialization;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Utils;

namespace CloudFlareDdns.Service
{
    public static class Config
    {
        public static readonly string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CloudFlareDDNS");

        public static readonly string LogFile = Path.Combine(Folder, "Logfile.txt");
        private static readonly string UserConfigFile = Path.Combine(Folder, "config.xml");

        public const string CloudFlareBaseUrl = "https://api.cloudflare.com/client/v4/";
        public const string IpUrl = "https://api.ipify.org?format=json";

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(UserConfig));

        public static UserConfig GetUserConfig()
        {
            try
            {
                if (File.Exists(UserConfigFile))
                {
                    using (FileStream fileStream = new FileStream(UserConfigFile, FileMode.Open))
                    {
                        return (UserConfig) Serializer.Deserialize(fileStream);
                    }
                }
                var config = new UserConfig();
                SetUserConfig(config);
                return config;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
                return null;
            }
        }

        public static void SetUserConfig(UserConfig config)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(UserConfig));
                TextWriter tw = new StreamWriter(UserConfigFile);
                xs.Serialize(tw, config);
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
            }
        }
    }
}