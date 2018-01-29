using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudFlareDDNS.Models;
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
    }
}