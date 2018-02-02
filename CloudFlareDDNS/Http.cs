using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudFlareDDNS.Models.Response;
using Newtonsoft.Json;

namespace CloudFlareDDNS
{
    public static class Http
    {
        public static async Task<IpResponse> GetPublicIp()
        {
            var publicIp = await HttpGetRequest<IpResponse>("https://api.ipify.org?format=json");
            publicIp.Updated = DateTime.UtcNow;
            return publicIp;
        }

        public static async Task<T> HttpGetRequest<T>(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<T>(responseBody);
                return responseObject;
            }
            return default(T);
        }

        public static async Task<T> CloudFlareHttpGetRequest<T>(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var cloudFlareBaseUrl = "https://api.cloudflare.com/client/v4/";
                client.DefaultRequestHeaders.Add("X-Auth-Key", "APIKEY");
                client.DefaultRequestHeaders.Add("X-Auth-Email", "EMAIL");
                HttpResponseMessage response = await client.GetAsync(cloudFlareBaseUrl + url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<T>(responseBody);
                    return responseObject;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
            }
            return default(T);
        }

        public static async Task<T> CloudFlareHttpPostRequest<T>(string url, T body)
        {
            try
            {
                HttpClient client = new HttpClient();
                var cloudFlareBaseUrl = "https://api.cloudflare.com/client/v4/";
                client.DefaultRequestHeaders.Add("X-Auth-Key", "APIKEY");
                client.DefaultRequestHeaders.Add("X-Auth-Email", "EMAIL");
                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(cloudFlareBaseUrl + url, data);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<T>(responseBody);
                    return responseObject;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
            }
            return default(T);
        }
    }
}