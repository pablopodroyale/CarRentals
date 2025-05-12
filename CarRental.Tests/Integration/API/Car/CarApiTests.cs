using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Tests.Integration.API.Car
{
    [TestFixture]
    internal class CarApiTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory
            {
                SeedAction = sp =>
                {
                    var db = sp.GetRequiredService<CarRentalDbContext>();
                    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                    var identityUser = userManager.FindByEmailAsync("admin@test.com").Result;

                    db.Cars.Add(new CarRental.Domain.Entities.Car
                    {
                        Type = "SUV",
                        Model = "Toyota",
                        Location = "Buenos Aires",
                        Services = new List<Service>
                        {
                            new Service
                            {
                                Date = DateTime.Today.AddMonths(-2).AddDays(7) // hace 1 mes y 23 días → AddMonths(2) = dentro de 7 días
                            }
                        }
                    });

                    db.SaveChanges();
                }
            };

            _client = _factory.CreateAdminClient();
        }

        [Test]
        public async Task Should_Return_UpcomingServiceCars()
        {
            // Act
            var response = await _client.GetAsync("/api/cars/upcoming-service");

            // Assert
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Does.Contain("Toyota"));
        }
    }
}
