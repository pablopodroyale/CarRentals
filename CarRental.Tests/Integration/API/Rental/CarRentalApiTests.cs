using CarRental.Domain;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace CarRental.Tests.Integration.API.Rental
{
    [TestFixture]
    public class CarRentalApiTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task Should_RegisterRental_Through_Api()
        {
            // Arrange: datos de prueba
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            var factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();

                    db.Customers.Add(new Domain.Entities.Customer { Id = customerId, FullName = "Test User", Address = "123 Main St" });
                    db.Cars.Add(new Car { Id = carId, Type = "SUV", Model = "Toyota" });

                    db.SaveChanges();
                }
            };

            var client = factory.CreateClient();

            var command = new
            {
                customerId,
                carType = "SUV",
                startDate = DateTime.Today,
                endDate = DateTime.Today.AddDays(3)
            };

            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json");

            // Act: se llama al endpoint
            var response = await client.PostAsync("/api/rentals", content);

            // Assert: verificamos éxito y presencia del rentalId
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("rentalId"));
        }
    }
}
