using CarRental.Functions.Email.Activities;
using CarRental.Shared.DTOs.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarRental.Tests.Functions.Email.Activities
{
    [TestFixture]
    public class SendEmailActivityFunctionTests
    {

        [Test]
        public async Task Should_Send_Email_Via_Activity()
        {
            // 1. Cargar la config desde appsettings.Development.json
            var jsonConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. Leer los valores desde el archivo
            var smtpHost = jsonConfig["Smtp:Host"];
            var smtpPort = jsonConfig["Smtp:Port"];
            var smtpUser = jsonConfig["Smtp:User"];
            var smtpPassword = jsonConfig["Smtp:Password"]; // ✅ viene del archivo

            // 3. Crear config final con los valores en memoria (sin hardcodearlos)
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Smtp:Host", smtpHost },
            { "Smtp:Port", smtpPort },
            { "Smtp:User", smtpUser },
            { "Smtp:Password", smtpPassword }
                })
                .Build();

            var logger = new Mock<ILogger<SendEmailActivityFunction>>();
            var function = new SendEmailActivityFunction(config);

            var input = new EmailRequestDto
            {
                To = smtpUser, // usar el mismo correo del archivo
                Body = "Test body",
                Subject = "Test subject"
            };

            Assert.DoesNotThrowAsync(() => function.Run(input));
        }

    }
}
