using CarRental.Shared.DTOs.Email;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarRental.Functions.Email.Activities
{
    public class SendEmailActivityFunction
    {
        private readonly IConfiguration _config;

        public SendEmailActivityFunction(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName("SendEmailActivity")]
        public async Task Run([ActivityTrigger] EmailRequestDto input)
        {
            var fromAddress = _config["Smtp:User"];
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"]!);
            var smtpPassword = _config["Smtp:Password"];

            using var mail = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = input.Subject,
                Body = input.Body,
                IsBodyHtml = true
            };

            mail.To.Add(input.To);

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, smtpPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
