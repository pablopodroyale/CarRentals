using CarRental.Functions.Email.Activities;
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
            // Arrange
            var config = new ConfigurationBuilder()
                     .AddInMemoryCollection(new Dictionary<string, string>
                     {
                    { "Smtp:Host", "smtp.gmail.com" },
                    { "Smtp:Port", "587" },
                    { "Smtp:User", "pablopodgaiz@gmail.com" },
                    { "Smtp:Password", "YOUR-API-KEY" }
                     }).Build();

            var logger = new Mock<ILogger<SendEmailActivityFunction>>();
            var function = new SendEmailActivityFunction(config);

            var input = new Shared.DTOs.Email.EmailRequestDto
            {
                To = "pablopodgaiz@gmail.com",
                Body = "Body",
                Subject = "Subject"
            };

            // Act & Assert
            Assert.DoesNotThrowAsync(() => function.Run(input));
        }
    }
}
