using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace VmAction
{
    public class AzureVMHelpers
    {
        public static async Task ListAllVMs(HttpClient client, string azSubscription, ILogger log)
        {
            JObject results = new JObject();

            string url = $"/subscriptions/{azSubscription}/providers/Microsoft.Compute/virtualMachines?api-version=2018-06-01";
            while (!String.IsNullOrEmpty(url))
            {
                using (var response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsAsync<dynamic>();
                    results.Merge(json);
                    url = json.nextLink;
                }
            }

            log.LogInformation(results.ToString());
        }


        public static async Task StopVM(HttpClient client, string azSubscription, string resourceGroupName, string vmName, ILogger log)
        {
            JObject results = new JObject();

            string url = $"/subscriptions/{azSubscription}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/virtualMachines/{vmName}/deallocate?api-version=2018-06-01";
            using (var response = await client.PostAsync(url, null))
            {
                response.EnsureSuccessStatusCode();
                results = JObject.FromObject(new { response.StatusCode });
            }

            log.LogInformation(results.ToString());
        }

        public static async Task StartVM(HttpClient client, string azSubscription, string resourceGroupName, string vmName, ILogger log)
        {
            JObject results = new JObject();

            string url = $"/subscriptions/{azSubscription}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/virtualMachines/{vmName}/start?api-version=2018-06-01";
            using (var response = await client.PostAsync(url, null))
            {
                response.EnsureSuccessStatusCode();
                results = JObject.FromObject(new { response.StatusCode });
            }

            log.LogInformation(results.ToString());
        }
    }
}
