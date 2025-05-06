using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Tests.Integration.API.Statistics
{
    [TestFixture]
    public class StatisticsApiTests
    {
        private HttpClient _client;

        [SetUp]
        public async Task Setup()
        {
            var factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();

                    var car1 = new Domain.Entities.Car { Id = Guid.NewGuid(), Type = "SUV", Model = "Toyota", Location = "Buenos Aires" };
                    var car2 = new Domain.Entities.Car { Id = Guid.NewGuid(), Type = "Sedan", Model = "Ford", Location = "Córdoba" };
                    db.Cars.AddRange(car1, car2);

                    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                    var identityUser = userManager.FindByEmailAsync("admin@test.com").Result;

                    var customer = new Domain.Entities.Customer
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@test.com",
                        ApplicationUserId = identityUser.Id,
                        FullName = "Admin User",
                        Address = new Address { Street = "Main St" }
                    };
                    db.Customers.Add(customer);

                    db.Rentals.AddRange(
                        new Domain.Entities.Rental(customer, car1, DateTime.Today.AddDays(-4), DateTime.Today.AddDays(-1)),
                        new Domain.Entities.Rental(customer, car1, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-8)),
                        new Domain.Entities.Rental(customer, car2, DateTime.Today.AddDays(-6), DateTime.Today.AddDays(-3))
                    );

                    db.SaveChanges();
                }
            };

            _client = factory.CreateAdminClient();
        }

        [Test]
        public async Task Should_Return_MostRentedCarType_Through_Api()
        {
            var response = await _client.GetAsync("/api/statistics/most-rented-type");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("SUV")); // ajusta según la data
        }

        [Test]
        public async Task Should_Return_UtilizationByLocation_Through_Api()
        {
            var response = await _client.GetAsync("/api/statistics/utilization-by-location");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Error {response.StatusCode}: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("Buenos Aires"));
            Assert.That(json, Does.Contain("Córdoba"));
        }
    }
}
