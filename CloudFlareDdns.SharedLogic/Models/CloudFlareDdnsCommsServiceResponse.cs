using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CloudFlareDdns.SharedLogic.Models
{
    public class CloudFlareDdnsCommsServiceBaseResponse 
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class UpdateResponse : CloudFlareDdnsCommsServiceBaseResponse
    {
        public string[] HostsUpdated { get; set; }
        public string[] HostsNotUpdated { get; set; }
    }
}
