using System.ServiceModel;

namespace CloudFlareDdns.Service.Interfaces
{
    public interface IServiceHostFactory
    {
        ServiceHost CreateServiceHost();
    }
}