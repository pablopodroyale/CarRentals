
using CarRental.Shared.DTOs.Email;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading.Tasks;

namespace CarRental.Functions.Email.Orchestrators
{
    public static class EmailOrchestratorFunction
    {
        [FunctionName("EmailOrchestrator")]
        public static async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<EmailRequestDto>();
            await context.CallActivityAsync("SendEmailActivity", input);
        }
    }
}
