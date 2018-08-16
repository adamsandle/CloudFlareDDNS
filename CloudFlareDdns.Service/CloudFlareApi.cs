using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.SharedLogic.Interfaces;
using CloudFlareDdns.SharedLogic.Models;

namespace CloudFlareDdns.Service
{
    public class CloudFlareApi
    {
        private readonly IOutputService _outputService;
        private readonly IHttpService _httpService;
        public CloudFlareApi(IOutputService outputService, IHttpService httpService)
        {
            _outputService = outputService;
            _httpService = httpService;
        }
        public async Task<UpdateResponse> UpdateDns(string[] hosts, string ipAddress)
        {
            List<string> hostsToUpdate = hosts.ToList();
            List<string> hostsUpdated = new List<string>();

            try
            {
                var zones = await _httpService.GetZones();

                var records = new List<CloudFlareDnsRecordsResultResponse>();

                foreach (var zone in zones.Result)
                {
                    var zoneRecords = await _httpService.GetRecords(zone.Id);
                    if (zoneRecords.Success)
                    {
                        records.AddRange(zoneRecords.Result);
                    }
                }

                var recordNamesToUpdate = records.Select(r => r.Name).Intersect(hostsToUpdate);
                var recordsToUpdate = records.Where(r => recordNamesToUpdate.Contains(r.Name));
                foreach (var record in recordsToUpdate)
                {
                    var recordDetails = await _httpService.GetRecordDetails(record.Zone_Id, record.Id);
                    var request = new CloudFlareUpdateDnsRecordRequest
                    {
                        name = recordDetails.Result.Name,
                        type = recordDetails.Result.Type,
                        proxied = recordDetails.Result.Proxied,
                        ttl = recordDetails.Result.Ttl,
                        content = ipAddress
                    };
                    var updateResult = await _httpService.UpdateRecord(record.Zone_Id, record.Id, request);
                    if (updateResult.Success)
                    {
                        _outputService.WriteLine(recordDetails.Result.Name + " Successfully Updated");

                        hostsToUpdate.Remove(recordDetails.Result.Name);
                        hostsUpdated.Add(recordDetails.Result.Name);
                    }
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
                _outputService.WriteLine(e);
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