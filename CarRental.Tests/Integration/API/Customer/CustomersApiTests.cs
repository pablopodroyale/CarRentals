using System.Text;
using System.Text.Json;

namespace CarRental.Tests.Integration.API.Customer
{
    [TestFixture]

    internal class CustomersApiTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var factory = new CustomWebApplicationFactory(); // Asegurate de tener esta clase configurada
            _client = factory.CreateClient();
        }

        [Test]
        public async Task Should_RegisterCustomer_ThroughApi()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateAuthenticatedClient();

            // 🔐 Autenticación
            //await TestAuthHelper.AuthenticateAsync(client, factory.Services);

            // Arrange
            var request = new
            {
                fullName = "Pablo Podgaiz",
                address = "Calle Siempreviva 742"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/customers", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("customerId"));
        }
    }
}
