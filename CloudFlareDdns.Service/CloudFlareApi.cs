using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.Service.Utils;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Service
{
    public static class CloudFlareApi
    {
        public static async Task<UpdateResponse> UpdateDns(IEnumerable<string> hosts)
        {
            List<string> hostsToUpdate = hosts.Any() ? hosts.ToList() : Config.GetUserConfig().HostsToUpdate.Select(c => c.Hostname).ToList();
            List<string> hostsUpdated = new List<string>();

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

                var recordNamesToUpdate = records.Select(r => r.Name).Intersect(hostsToUpdate);
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

                    hostsToUpdate.Remove(recordDetails.Result.Name);
                    hostsUpdated.Add(recordDetails.Result.Name);
                }
                return new UpdateResponse
                {
                    HostsNotUpdated = hostsToUpdate.ToArray(),
                    HostsUpdated = hostsUpdated.ToArray(),
                    Success = hostsToUpdate.Count == 0
                };
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
                return new UpdateResponse
                {
                    HostsNotUpdated = hostsToUpdate.ToArray(),
                    HostsUpdated = hostsUpdated.ToArray(),
                    Success = false
                };
            }
        }
    }
}