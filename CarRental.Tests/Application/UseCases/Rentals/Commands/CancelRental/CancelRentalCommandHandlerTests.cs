using CarRental.Application.Interfaces;
using CarRental.Application.UseCases.Rentals.Commands.CancelRental;
using Moq;
using MediatR;
using CarRental.Domain.Entities;

namespace CarRental.Tests.Application.UseCases.Rentals.Commands.CancelRental
{
    [TestFixture]
    public class CancelRentalCommandHandlerTests
    {
        private Mock<IRentalRepository> _mockRepository;
        private CancelRentalCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRentalRepository>();
            _handler = new CancelRentalCommandHandler(_mockRepository.Object);
        }

        [Test]
        public async Task Should_CancelRental_When_RentalExists()
        {
            string location = "EZE";
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "john@example.com",
                ApplicationUserId = "user-123",
                Address = new Address
                {
                    Street = "Main St",
                }
            };

            var car = new Car
            {
                Id = Guid.NewGuid(),
                Type = "SUV",
                Model = "Toyota RAV4",
                Location = location
            };

            var rental = new Rental(customer, car, DateTime.Today, DateTime.Today.AddDays(1), location);

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(rental.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rental);

            _mockRepository
                .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CancelRentalCommand { RentalId = rental.Id };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.EqualTo(Unit.Value));
            Assert.That(rental.IsCanceled, Is.True);
        }

        [Test]
        public void Should_Throw_When_RentalNotFound()
        {
            var rentalId = Guid.NewGuid();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(rentalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rental)null!);

            var command = new CancelRentalCommand { RentalId = rentalId };

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );

            Assert.That(ex!.Message, Is.EqualTo("Rental not found"));
        }
    }
}
