using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace VmAction
{
    public static class ControlVM
    {
        [FunctionName("ControlVM")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string resourceGroupName = req.Query["resourcegroupname"];
            string vmName = req.Query["name"];
            string vmAction = req.Query["action"];
            string azSubscription = req.Query["subscription"];

            string clientId = Environment.GetEnvironmentVariable("ClientId");
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            string tenantId = Environment.GetEnvironmentVariable("TenantId");

            var token = await AuthenticationHelpers.AcquireTokenBySPN(tenantId, clientId, clientSecret, log);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.BaseAddress = new Uri("https://management.azure.com/");

                //await AzureVMHelpers.ListAllVMs(client, azSubscription, log);

                if (vmAction == "stop")
                {
                    await AzureVMHelpers.StopVM(client, azSubscription, resourceGroupName, vmName, log);
                }
                if (vmAction == "start")
                {
                    await AzureVMHelpers.StartVM(client, azSubscription, resourceGroupName, vmName, log);
                }
            }

            return vmName != null
                ? (ActionResult)new OkObjectResult($"VM: {vmName} in RG: {resourceGroupName} and subscription {azSubscription} will {vmAction}")
                : new BadRequestObjectResult("Oops, something went wrong");
        }
    }
}