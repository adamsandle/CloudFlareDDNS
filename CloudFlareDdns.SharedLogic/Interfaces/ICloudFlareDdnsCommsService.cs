using System.Collections.Generic;
using System.ServiceModel;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.SharedLogic.Interfaces
{
    [ServiceContract]
    public interface ICloudFlareDdnsCommsService
    {
        [OperationContract]
        GetIpResponse GetIp();

        [OperationContract]
        UpdateResponse ForceUpdate(string[] hosts);
    }
}
