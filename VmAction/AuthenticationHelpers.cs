using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VmAction
{
    public class AuthenticationHelpers
    {
        const string armEndpoint = "https://management.azure.com/";
        const string tokenEndpoint = "https://login.microsoftonline.com/{0}/oauth2/token";
        const string spnPayload = "resource={0}&client_id={1}&grant_type=client_credentials&client_secret={2}";

        public static async Task<string> AcquireTokenBySPN(string tenantId, string clientId, string clientSecret, ILogger log)
        {
            var payload = String.Format(spnPayload,
                                        WebUtility.UrlEncode(armEndpoint),
                                        WebUtility.UrlEncode(clientId),
                                        WebUtility.UrlEncode(clientSecret));

            var body = await HttpPost(tenantId, payload, log);
            return body.access_token;
        }

        static async Task<dynamic> HttpPost(string tenantId, string payload, ILogger log)
        {
            using (var client = new HttpClient())
            {
                var address = String.Format(tokenEndpoint, tenantId);
                var content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");
                using (var response = await client.PostAsync(address, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        log.LogError("Status:  {0}", response.StatusCode);
                        log.LogError("Content: {0}", await response.Content.ReadAsStringAsync());
                    }

                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsAsync<dynamic>();
                }
            }
        }
    }
}
