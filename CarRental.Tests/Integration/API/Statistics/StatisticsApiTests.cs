using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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

                var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                var identityUser = userManager.FindByEmailAsync("admin@test.com").Result;

                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@test.com",
                    ApplicationUserId = identityUser.Id,
                    FullName = "Admin User",
                    Address = new Address { Street = "Main St" }
                };
                db.Customers.Add(customer);

                db.Rentals.AddRange(
                    new Rental(customer, car1, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-8)),
                    new Rental(customer, car1, DateTime.Today.AddDays(-4), DateTime.Today.AddDays(-1)),
                    new Rental(customer, car2, DateTime.Today.AddDays(-6), DateTime.Today.AddDays(-3))
                );

                db.SaveChanges();
            }
        };

        _client = factory.CreateAdminClient();
    }

    [Test]
    public async Task Should_Return_MostRentedCarType_Through_Api()
    {
        var from = DateTime.Today.AddDays(-14).ToString("yyyy-MM-dd");
        var to = DateTime.Today.ToString("yyyy-MM-dd");

        var response = await _client.GetAsync($"/api/statistics/most-rented?from={from}&to={to}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.That(json, Does.Contain("SUV"));
    }

    [Test]
    public async Task Should_Return_UtilizationByType_Through_Api()
    {
        var from = DateTime.Today.AddDays(-14).ToString("yyyy-MM-dd");
        var to = DateTime.Today.ToString("yyyy-MM-dd");

        var response = await _client.GetAsync($"/api/statistics/utilization?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Error {response.StatusCode}: {error}");
        }

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("RESPONSE: " + json); // opcional

        Assert.That(json, Does.Contain("\"type\":\"SUV\""));
        Assert.That(json, Does.Contain("\"type\":\"Sedan\""));
        Assert.That(json, Does.Contain("\"percentageUsed\":100"));
    }

}
