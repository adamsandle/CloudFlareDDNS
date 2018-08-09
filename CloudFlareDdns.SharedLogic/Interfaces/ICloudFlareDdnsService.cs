using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.SharedLogic.Interfaces
{
    [ServiceContract]
    public interface ICloudFlareDdnsCommsService
    {
        [OperationContract]
        string GetIp();

        [OperationContract]
        UpdateResponse ForceUpdate(string[] hosts);
    }
}
