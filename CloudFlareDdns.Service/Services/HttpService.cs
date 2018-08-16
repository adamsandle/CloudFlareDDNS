using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudFlareDdns.Service.Interfaces;
using CloudFlareDdns.Service.Models;
using CloudFlareDdns.Service.Models.Requests;
using CloudFlareDdns.Service.Models.Response;
using CloudFlareDdns.SharedLogic.Interfaces;
using Newtonsoft.Json;

namespace CloudFlareDdns.Service.Services
{
    public class HttpService : IHttpService
    {
        private readonly IOutputService _outputService;
        private readonly HttpClient _httpClient = new HttpClient();
        private string _cloudFlareEmail;
        private string _cloudFlareApiKey;
        private const string CloudFlareBaseUrl = "https://api.cloudflare.com/client/v4/";
        private const string IpUrl = "https://api.ipify.org?format=json";

        public HttpService(IOutputService outputService)
        {
            _outputService = outputService;
            
        }

        public void CloudFlareCredentialsUpdated(string email, string key)
        {
            _cloudFlareEmail = email;
            _cloudFlareApiKey = key;
        }

        public async Task<IpResponse> GetPublicIp()
        {
            try
            {
                var publicIp = await HttpRequest<IpResponse>(HttpMethod.Get, IpUrl, false, null);
                if (publicIp != null)
                {
                    publicIp.Updated = DateTime.UtcNow;
                }
                return publicIp;
            }
            catch (Exception e)
            {
                _outputService.WriteLine(e);
            }
            return null;
        }

        public Task<CloudFlareZonesResponse> GetZones()
        {
            return HttpRequest<CloudFlareZonesResponse>(HttpMethod.Get, "zones", true, null);
        }

        public Task<CloudFlareDnsRecordsResponse> GetRecords(string zoneId)
        {
            return HttpRequest<CloudFlareDnsRecordsResponse>(HttpMethod.Get, "zones/" + zoneId + "/dns_records", true, null);
        }

        public Task<CloudFlareDnsRecordResponse> GetRecordDetails(string zoneId, string recordId)
        {
            return HttpRequest<CloudFlareDnsRecordResponse>(HttpMethod.Get, "zones/" + zoneId + "/dns_records/" + recordId, true, null);
        }

        public Task<CloudFlareBaseResponse> UpdateRecord(string zoneId, string recordId, CloudFlareUpdateDnsRecordRequest request)
        {
            return HttpRequest<CloudFlareBaseResponse>(HttpMethod.Put, "zones/" + zoneId + "/dns_records/" + recordId, true, request);
        }

        private async Task<T> HttpRequest<T>(HttpMethod method, string url, bool cloudFlare, BaseRequest body)
        {
            var request = new HttpRequestMessage(method, cloudFlare ? CloudFlareBaseUrl + url : url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (cloudFlare)
            {
                request.Headers.Add("X-Auth-Key", _cloudFlareApiKey);
                request.Headers.Add("X-Auth-Email", _cloudFlareEmail);
            }

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = data;
            }

            var response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<T>(responseBody);

            if (cloudFlare)
            {
                var baseResponse = responseObject as CloudFlareBaseResponse;
                if (baseResponse != null)
                {
                    foreach (var error in baseResponse.Errors)
                    {
                        _outputService.WriteLine("CloudFlare API Error: " + error.Code + " " + error.Message);
                    }
                    foreach (var message in baseResponse.Messages)
                    {
                        _outputService.WriteLine("CloudFlare API Message: " + message.Code + " " + message.Message);
                    }
                }
            }
            if (response.IsSuccessStatusCode)
            {
                return responseObject;
            }
            throw new Exception(response.StatusCode.ToString());
        }
    }
}