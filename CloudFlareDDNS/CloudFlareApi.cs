using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudFlareDDNS.Models.Requests;
using CloudFlareDDNS.Models.Response;

namespace CloudFlareDDNS
{
    public static class CloudFlareApi
    {
        public static async Task<bool> UpdateDns()
        {
            try
            {
                var zones = await Http.HttpRequest<CloudFlareZonesResponse>(HttpMethod.Get, "zones", true, null);

                var records = new List<CloudFlareDnsRecordsResultResponse>();

                foreach (var zone in zones.Result)
                {
                    var zoneRecords = await Http.HttpRequest<CloudFlareDnsRecordsResponse>(HttpMethod.Get, "zones/" + zone.Id + "/dns_records", true, null);
                    if (zoneRecords.Success)
                    {
                        records.AddRange(zoneRecords.Result);
                    }
                }

                var recordNamesToUpdate = records.Select(r => r.Name).Intersect(CloudFlareDdnsService.UserConfig.HostsToUpdate.Select(r => r.Hostname));
                var recordsToUpdate = records.Where(r => recordNamesToUpdate.Contains(r.Name));
                foreach (var record in recordsToUpdate)
                {
                    var recordDetails = await Http.HttpRequest<CloudFlareDnsRecordResponse>(HttpMethod.Get, "zones/" + record.Zone_Id + "/dns_records/" + record.Id, true, null);
                    var request = new CloudFlareUpdateDnsRecordRequest
                    {
                        name = recordDetails.Result.Name,
                        type = recordDetails.Result.Type,
                        proxied = recordDetails.Result.Proxied,
                        ttl = recordDetails.Result.Ttl,
                        content = CloudFlareDdnsService.IpResponse.Ip
                    };
                    await Http.HttpRequest<CloudFlareBaseResponse>(HttpMethod.Put, "zones/" + record.Zone_Id + "/dns_records/" + record.Id, true, request);
                    Logger.WriteLog(recordDetails.Result.Name + " Successfully Updated");
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
                return false;
            }
        }
    }
}