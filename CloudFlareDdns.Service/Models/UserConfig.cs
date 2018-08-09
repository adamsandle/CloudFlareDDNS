using System.Xml.Serialization;

namespace CloudFlareDdns.Service.Models
{
    [XmlRoot("config")]
    public class UserConfig
    {
        [XmlElement("email")]
        public string Email { get; set; }
        [XmlElement("apiKey")]
        public string ApiKey { get; set; }
        [XmlArray("hosts")]
        [XmlArrayItem("host")]
        public string[] Hosts { get; set; }
    }
}