using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudFlareDDNS.Models.Requests;
using CloudFlareDDNS.Models.Response;
using Newtonsoft.Json;

namespace CloudFlareDDNS
{
    public static class Http
    {
        public static async Task<IpResponse> GetPublicIp()
        {
            try
            {
                var publicIp = await HttpRequest<IpResponse>(HttpMethod.Get, Config.IpUrl, false, null);
                if (publicIp != null)
                {
                    publicIp.Updated = DateTime.UtcNow;
                }
                return publicIp;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
            }
            return null;
        }

        private static HttpClient client = new HttpClient();
        public static async Task<T> HttpRequest<T>(HttpMethod method, string url, bool cloudFlare, BaseRequest body)
        {
            var request = new HttpRequestMessage(method, cloudFlare ? Config.CloudFlareBaseUrl + url : url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (cloudFlare)
            {
                request.Headers.Add("X-Auth-Key", CloudFlareDdnsService.UserConfig.ApiKey);
                request.Headers.Add("X-Auth-Email", CloudFlareDdnsService.UserConfig.Email);
            }

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = data;
            }

            var response = await client.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<T>(responseBody);

            if (cloudFlare)
            {
                var baseResponse = responseObject as CloudFlareBaseResponse;
                if (baseResponse != null)
                {
                    foreach (var error in baseResponse.Errors)
                    {
                        Logger.WriteLog("CloudFlare API Error: " + error.Code + " " + error.Message);
                    }
                    foreach (var message in baseResponse.Messages)
                    {
                        Logger.WriteLog("CloudFlare API Message: " + message.Code + " " + message.Message);
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