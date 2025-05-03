using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

                    var car1 = new Car { Id = Guid.NewGuid(), Type = "SUV", Model = "Toyota", Location = "Buenos Aires" };
                    var car2 = new Car { Id = Guid.NewGuid(), Type = "Sedan", Model = "Ford", Location = "Córdoba" };
                    db.Cars.AddRange(car1, car2);

                    var customer = new Domain.Entities.Customer { Id = Guid.NewGuid(), FullName = "Test User", Address = "123 Main St" };
                    db.Customers.Add(customer);

                    db.Rentals.AddRange(
                        new Domain.Entities.Rental(customer.Id, car1.Id, DateTime.Today.AddDays(-4), DateTime.Today.AddDays(-1)),
                        new Domain.Entities.Rental(customer.Id, car1.Id, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-8)),
                        new Domain.Entities.Rental(customer.Id, car2.Id, DateTime.Today.AddDays(-6), DateTime.Today.AddDays(-3))
                    );

                    db.SaveChanges();
                }
            };

            _client = factory.CreateAuthenticatedClient();
            //await TestAuthHelper.AuthenticateAsync(_client, factory.Services);
        }

        [Test]
        public async Task Should_Return_MostRentedCarType_Through_Api()
        {
            try
            {
                var response = await _client.GetAsync("/api/statistics/most-rented-type");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Assert.That(json, Does.Contain("SUV")); // ajusta según la data
            }
            catch (Exception)
            {
                throw;
            }
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
