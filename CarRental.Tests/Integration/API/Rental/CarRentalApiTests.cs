using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
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
        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Test]
        public async Task Should_RegisterRental_Through_Api()
        {
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            _factory.SeedAction = sp =>
            {
                var db = sp.GetRequiredService<CarRentalDbContext>();
                var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                var identityUser = userManager.FindByEmailAsync("user@test.com").Result;

                db.Customers.Add(new Domain.Entities.Customer
                {
                    Id = customerId,
                    Email = "user@test.com",
                    ApplicationUserId = identityUser.Id,
                    FullName = "Test User",
                    Address = new Address { Street = "Av. Siempre Viva" }
                });

                db.Cars.Add(new Domain.Entities.Car
                {
                    Id = carId,
                    Type = "SUV",
                    Model = "Toyota",
                    Location = "Buenos Aires"
                });

                db.SaveChanges();
            };

            _client = _factory.CreateUserClient();

            var command = new
            {
                customerId,
                carType = "SUV",
                startDate = DateTime.Today,
                endDate = DateTime.Today.AddDays(3)
            };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/rentals", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("rentalId"));
        }

        [Test]
        public async Task Should_ModifyRental_Through_Api()
        {
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var oldCarId = Guid.NewGuid();
            var newCarId = Guid.NewGuid();

            _factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();
                    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                    var identityUser = userManager.FindByEmailAsync("user@test.com").Result;

                    var customer = new Domain.Entities.Customer
                    {
                        Id = customerId,
                        Email = "user@test.com",
                        ApplicationUserId = identityUser.Id,
                        FullName = "Test User",
                        Address = new Address { Street = "Av. Siempre Viva" }
                    };

                    var oldCar = new Domain.Entities.Car { Id = oldCarId, Type = "Sedan", Model = "Ford Focus", Location = "Buenos Aires" };
                    var newCar = new Domain.Entities.Car { Id = newCarId, Type = "SUV", Model = "Honda CR-V", Location = "Córdoba" };

                    var rental = new Domain.Entities.Rental(customer, oldCar, DateTime.Today, DateTime.Today.AddDays(2));
                    typeof(Domain.Entities.Rental).GetProperty("Id")!.SetValue(rental, rentalId);

                    db.Customers.Add(customer);
                    db.Cars.AddRange(oldCar, newCar);
                    db.Rentals.Add(rental);
                    db.SaveChanges();
                }
            };

            _client = _factory.CreateAdminClient();

            var command = new
            {
                rentalId,
                carId = newCarId,
                newStartDate = DateTime.Today.AddDays(1),
                newEndDate = DateTime.Today.AddDays(4)
            };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/rentals/{rentalId}", content);
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Should_CancelRental_Through_Api()
        {
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            _factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();
                    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                    var identityUser = userManager.FindByEmailAsync("admin@test.com").Result;

                    var customer = new Domain.Entities.Customer
                    {
                        Id = customerId,
                        Email = "admin@test.com",
                        ApplicationUserId = identityUser.Id,
                        FullName = "Admin User",
                        Address = new Address { Street = "Main St" }
                    };

                    db.Customers.Add(customer);
                    db.Cars.Add(new Domain.Entities.Car { Id = carId, Type = "SUV", Model = "Toyota", Location = "Buenos Aires" });
                    db.SaveChanges();
                }
            };

            _client = _factory.CreateAdminClient();

            var command = new
            {
                customerId,
                carType = "SUV",
                startDate = DateTime.Today,
                endDate = DateTime.Today.AddDays(3)
            };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/rentals", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("rentalId"));
        }
    }
}
