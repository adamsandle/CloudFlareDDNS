using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudFlareDDNS.Models.Requests;
using CloudFlareDDNS.Models.Response;

namespace CloudFlareDDNS
{
    public static class CloudFlareApi
    {
        public static async Task UpdateDns()
        {
            var zones = await Http.CloudFlareHttpGetRequest<CloudFlareZonesResponse>("zones");

            var records = new List<CloudFlareDnsRecordsResultResponse>();

            foreach (var zone in zones.Result)
            {
                var zoneRecords = await Http.CloudFlareHttpGetRequest<CloudFlareDnsRecordsResponse>("zones/" + zone.Id + "/dns_records");
                if (zoneRecords.Success)
                {
                    records.AddRange(zoneRecords.Result);
                }
            }

            var recordNamesToUpdate = records.Select(r => r.Name).Intersect(CloudFlareDdnsService.UserConfig.HostsToUpdate.Select(r => r.Hostname));
            var recordsToUpdate = records.Where(r => recordNamesToUpdate.Contains(r.Name));
            foreach (var record in recordsToUpdate)
            {
                var recordDetails = await Http.CloudFlareHttpGetRequest<CloudFlareDnsRecordResponse>("zones/" + record.Zone_Id + "/dns_records/" + record.Id);
                var request = new CloudFlareUpdateDnsRecordRequest
                {
                    name = recordDetails.Result.Name,
                    type = recordDetails.Result.Type,
                    proxied = recordDetails.Result.Proxied,
                    ttl = recordDetails.Result.Ttl,
                    content = CloudFlareDdnsService.IpResponse.Ip
                };
                await Http.CloudFlareHttpPutRequest("zones/" + record.Zone_Id + "/dns_records/" + record.Id, request);
            }
        }
    }
}