using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Text.Json;

namespace CarRental.Tests.Integration.API.Customer
{
    [TestFixture]

    internal class CustomersApiTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Test]
        public async Task Should_RegisterCustomer_ThroughApi()
        {
            // Arrange
            RegisterCustomerCommand request = new RegisterCustomerCommand()
            {
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Test1234$",
                Address = "123 Main St"
            };


            _client = _factory.CreateUserClient();

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/customers", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("customerId"));
        }
    }
}
