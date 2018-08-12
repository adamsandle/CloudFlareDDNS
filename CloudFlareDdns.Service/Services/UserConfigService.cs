using System;
using System.IO;
using System.Xml.Serialization;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Service.Services
{
    public class UserConfigService : IUserConfigService
    {
        private readonly string _configFile;
        private readonly string _folder;
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(UserConfig));
        private readonly IOutputService _outputService;

        public UserConfigService(IOutputService outputService, string folder)
        {
            _outputService = outputService;
            _folder = folder;
            _configFile = Path.Combine(folder, "config.xml");
        }

        public void CreateFolder()
        {
            Directory.CreateDirectory(_folder);
        }

        public UserConfig GetUserConfig()
        {
            try
            {
                if (File.Exists(_configFile))
                {
                    using (FileStream fileStream = new FileStream(_configFile, FileMode.Open))
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
                _outputService.WriteLine(e);
                return null;
            }
        }

        public void SetUserConfig(UserConfig config)
        {
            try
            {
//                XmlSerializer xs = new XmlSerializer(typeof(UserConfig));
                TextWriter tw = new StreamWriter(_configFile);
                Serializer.Serialize(tw, config);
            }
            catch (Exception e)
            {
                _outputService.WriteLine(e);
            }
        }
    }
}