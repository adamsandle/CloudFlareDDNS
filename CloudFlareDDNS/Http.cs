using System;
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

        public static async Task<T> HttpRequest<T>(HttpMethod method, string url, bool cloudFlare, BaseRequest body)
        {
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage(method, url);
            
            if (cloudFlare)
            {
                client.DefaultRequestHeaders.Add("X-Auth-Key", CloudFlareDdnsService.UserConfig.ApiKey);
                client.DefaultRequestHeaders.Add("X-Auth-Email", CloudFlareDdnsService.UserConfig.Email);
                client.BaseAddress = new Uri(Config.CloudFlareBaseUrl);
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
            if (response.IsSuccessStatusCode)
            {
                return responseObject;
            }
            else if (cloudFlare)
            {
                var baseResponse = responseObject as CloudFlareBaseResponse;
                if (baseResponse != null)
                {
                    foreach (var error in baseResponse.Errors)
                    {
                        Logger.WriteLog("CloudFlare API Error: " + error.Code + " " + error.Message);
                    }
                }
            }
            throw new Exception(response.StatusCode.ToString());
        }
    }
}