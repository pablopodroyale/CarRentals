using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using CarRental.Shared.DTOs.Email;

namespace CarRental.Functions.Email.Triggers
{
    public static class StartEmailOrchestrationFunction
    {
        [FunctionName("StartEmailOrchestration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            log.LogInformation("HTTP trigger received to start orchestration.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<EmailRequestDto>(requestBody);

            if (data == null || string.IsNullOrWhiteSpace(data.To))
            {
                return new BadRequestObjectResult("Please provide valid email data.");
            }

            string instanceId = await client.StartNewAsync("EmailOrchestrator", data);

            log.LogInformation("Orchestration started with ID = {InstanceId}", instanceId);

            return new OkObjectResult($"Orchestration started with ID = {instanceId}");
        }
    }
}
