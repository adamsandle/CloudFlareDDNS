using System.Threading.Tasks;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;

namespace CloudFlareDdns.Service.Interfaces
{
    public interface IHttpService
    {
        void CloudFlareCredentialsUpdated(string email, string key);
        Task<IpResponse> GetPublicIp();
        Task<CloudFlareZonesResponse> GetZones();
        Task<CloudFlareDnsRecordsResponse> GetRecords(string zoneId);
        Task<CloudFlareDnsRecordResponse> GetRecordDetails(string zoneId, string recordId);
        Task<CloudFlareBaseResponse> UpdateRecord(string zoneId, string recordId, CloudFlareUpdateDnsRecordRequest request);
    }
}
