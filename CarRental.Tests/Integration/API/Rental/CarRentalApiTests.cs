using CarRental.Domain;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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

        [Test]
        public async Task Should_ModifyRental_Through_Api()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var oldCarId = Guid.NewGuid();
            var newCarId = Guid.NewGuid();

            var factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();

                    db.Customers.Add(new Domain.Entities.Customer { Id = customerId, FullName = "Mod Test", Address = "456 Change St" });
                    db.Cars.AddRange(
                        new Car { Id = oldCarId, Type = "Sedan", Model = "Ford Focus" },
                        new Car { Id = newCarId, Type = "SUV", Model = "Honda CR-V" });

                    var rental = new Domain.Entities.Rental(customerId, oldCarId, DateTime.Today, DateTime.Today.AddDays(2));
                    typeof(Domain.Entities.Rental).GetProperty("Id")!.SetValue(rental, rentalId);
                    db.Rentals.Add(rental);
                    db.SaveChanges();
                }
            };

            var client = factory.CreateClient();

            var command = new
            {
                rentalId = rentalId,
                carId = newCarId,
                newStartDate = DateTime.Today.AddDays(1),
                newEndDate = DateTime.Today.AddDays(4)
            };

            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PutAsync($"/api/rentals/{rentalId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Should_CancelRental_Through_Api()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            var factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();
                    db.Customers.Add(new Domain.Entities.Customer { Id = customerId, FullName = "Cancel Test", Address = "789 Cancel St" });
                    db.Cars.Add(new Car { Id = carId, Type = "SUV", Model = "Kia Sportage" });

                    var rental = new Domain.Entities.Rental(customerId, carId, DateTime.Today, DateTime.Today.AddDays(3));
                    typeof(Domain.Entities.Rental).GetProperty("Id")!.SetValue(rental, rentalId);
                    db.Rentals.Add(rental);
                    db.SaveChanges();
                }
            };

            var client = factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/api/rentals/{rentalId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
