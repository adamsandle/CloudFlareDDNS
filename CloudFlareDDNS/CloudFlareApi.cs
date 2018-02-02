using System.Collections.Generic;
using System.Linq;
using CloudFlareDDNS.Models.Requests;
using CloudFlareDDNS.Models.Response;

namespace CloudFlareDDNS
{
    public static class CloudFlareApi
    {
        public static async void UpdateDns(string ipAddress)
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

            var recordsToUpdate = records.Where(r => r.Name == "domain.com");
            foreach (var record in recordsToUpdate)
            {
                var request = new CloudFlareUpdateDnsRecordRequest
                {
                    name = record.Name,
                    type = record.Type,
                    content = ipAddress
                };
                await Http.CloudFlareHttpPostRequest("zones/" + record.Zone_Id + "/dns_records/" + record.Id, request);
            }
        }
    }
}