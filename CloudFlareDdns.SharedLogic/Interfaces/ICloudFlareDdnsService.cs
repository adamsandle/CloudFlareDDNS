using System.ServiceModel;

namespace CloudFlareDdns.SharedLogic.Interfaces
{
    [ServiceContract]
    public interface ICloudFlareDdnsCommsService
    {
        [OperationContract]
        string GetIp();
    }
}
