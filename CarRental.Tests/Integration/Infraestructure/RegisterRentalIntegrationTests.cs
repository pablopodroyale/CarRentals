using CarRental.Application.Interfaces;
using CarRental.Application.UseCases;
using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class RegisterRentalIntegrationTests
{
    private IServiceProvider _sp;
    private CarRentalDbContext _db;
    private Guid _customerId;
    private Customer _customer;
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
                var identityUser = userManager.FindByEmailAsync("user@test.com").Result;

                _customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = identityUser.Email,
                    ApplicationUserId = identityUser.Id,
                    FullName = "Test User",
                    Address = new Address { Street = "123 Main" }
                };
                _customerId = _customer.Id;

                db.Customers.Add(_customer);

                db.Cars.Add(new Car
                {
                    Id = Guid.NewGuid(),
                    Type = "SUV",
                    Model = "Toyota RAV4",
                    Location = "Buenos Aires",
                    Services = new List<Service>()
                });

                db.SaveChanges();
            }
        };

        var client = _factory.CreateUserClient();

        using var scope = _factory.Services.CreateScope();
        _sp = scope.ServiceProvider;
        _db = _sp.GetRequiredService<CarRentalDbContext>();
    }

    [Test]
    public async Task Should_RegisterRental_When_CarIsAvailable()
    {
        using var scope = _factory.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var db = sp.GetRequiredService<CarRentalDbContext>();
        var rentalService = sp.GetRequiredService<IRentalService>();
        var useCase = new RegisterRentalUseCase(rentalService);
        CancellationToken cancellationToken = new CancellationToken();
        var rentalId = await useCase.ExecuteAsync(_customerId, "SUV", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken);
        var rental = await db.Rentals.Include(r => r.Customer).FirstOrDefaultAsync(r => r.Id == rentalId);

        Assert.That(rental, Is.Not.Null);
        Assert.That(rental.Customer.Id, Is.EqualTo(_customerId));
    }

    [Test]
    public async Task Should_Throw_When_CarAlreadyReserved()
    {
        // Arrange - inicializar todo desde 0
        var factory = new CustomWebApplicationFactory
        {
            SeedAction = sp =>
            {
                var db = sp.GetRequiredService<CarRentalDbContext>();
                var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                var identityUser = userManager.FindByEmailAsync("user@test.com").Result;

                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = identityUser.Email,
                    ApplicationUserId = identityUser.Id,
                    FullName = "Test User",
                    Address = new Address { Street = "123 Main" }
                };

                var car = new Car
                {
                    Id = Guid.NewGuid(),
                    Type = "SUV",
                    Model = "Toyota RAV4",
                    Location = "Buenos Aires",
                    Services = new List<Service>()
                };

                db.Customers.Add(customer);
                db.Cars.Add(car);
                db.Rentals.Add(new Rental(customer, car, DateTime.Today, DateTime.Today.AddDays(3)));

                db.SaveChanges();
            }
        };

        var client = factory.CreateUserClient();

        using var scope = factory.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var rentalService = sp.GetRequiredService<IRentalService>();
        var customer = await sp.GetRequiredService<CarRentalDbContext>().Customers.FirstAsync();
        var useCase = new RegisterRentalUseCase(rentalService);
        var cancellationToken = new CancellationToken();

        // Act & Assert
        var ex = Assert.ThrowsAsync<NoCarAvailableException>(() =>
            useCase.ExecuteAsync(customer.Id, "SUV", DateTime.Today, DateTime.Today.AddDays(2), cancellationToken));

        Assert.That(ex!.Message, Is.EqualTo("No car available for the selected dates."));
    }
}
