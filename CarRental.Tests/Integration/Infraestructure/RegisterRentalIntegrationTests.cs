using Microsoft.EntityFrameworkCore;
using CarRental.Application.UseCases;
using CarRental.Infrastructure.Persistence;
using CarRental.Domain.Entities;
using CarRental.Infrastructure;
using CarRental.Domain;
using CarRental.Domain.Exceptions;

namespace CarRental.Tests.Integration.Infraestructure
{
    [TestFixture]
    public class RegisterRentalIntegrationTests
    {
        [Test]
        public async Task Should_RegisterRental_When_CarIsAvailable()
        {
            // Setup InMemory DbContext
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CarRentalDbContext(options);

            // Arrange
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            context.Customers.Add(new Customer
            {
                Id = customerId,
                FullName = "Juan",
                Address = "Calle 123"
            });

            context.Cars.Add(new Car
            {
                Id = carId,
                Type = "SUV",
                Model = "Toyota RAV4"
            });

            await context.SaveChangesAsync();

            var rentalService = new RentalService(context);
            var useCase = new RegisterRentalUseCase(rentalService);

            // Act
            var rentalId = await useCase.ExecuteAsync(
                customerId,
                "SUV",
                DateTime.Today,
                DateTime.Today.AddDays(3));

            // Assert
            var rental = await context.Rentals.FindAsync(rentalId);

            Assert.That(rental, Is.Not.Null);
            Assert.That(rental.CustomerId, Is.EqualTo(customerId));
            Assert.That(rental.CarId, Is.EqualTo(carId));
        }

        [Test]
        public async Task Should_Throw_When_CarAlreadyReserved()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CarRentalDbContext(options);

            var customer1Id = Guid.NewGuid();
            var customer2Id = Guid.NewGuid();
            var carId = Guid.NewGuid();

            context.Customers.AddRange(
                new Customer { Id = customer1Id, FullName = "Juan", Address = "X" },
                new Customer { Id = customer2Id, FullName = "Ana", Address = "Y" });

            context.Cars.Add(new Car { Id = carId, Type = "SUV", Model = "Toyota" });

            context.Rentals.Add(new Rental
            (
                customer1Id,
                carId,
                DateTime.Today,
                DateTime.Today.AddDays(3)
            ));

            await context.SaveChangesAsync();

            var rentalService = new RentalService(context);
            var useCase = new RegisterRentalUseCase(rentalService);

            // Act + Assert
            var ex = Assert.ThrowsAsync<NoCarAvailableException>(async () =>
            {
                await useCase.ExecuteAsync(customer2Id, "SUV", DateTime.Today, DateTime.Today.AddDays(2));
            });

            Assert.That(ex.Message, Is.EqualTo("No car available for the selected dates."));
        }
    }
}
