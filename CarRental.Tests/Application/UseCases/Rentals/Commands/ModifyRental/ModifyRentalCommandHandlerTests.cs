using CarRental.Application.Interfaces;
using CarRental.Application.UseCases.Rentals.Commands.ModifyRental;
using CarRental.Domain.Entities;
using MediatR;
using Moq;

namespace CarRental.Tests.UseCases.Rentals.Commands
{
    [TestFixture]
    public class ModifyRentalCommandHandlerTests
    {
        private Mock<IRentalRepository> _rentalRepositoryMock;
        private Mock<ICarRepository> _carRepositoryMock;
        private ModifyRentalCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _carRepositoryMock = new Mock<ICarRepository>();
            _handler = new ModifyRentalCommandHandler(_rentalRepositoryMock.Object, _carRepositoryMock.Object);
        }

        [Test]
        public async Task Should_ModifyRental_When_RentalExists()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();
            var rental = new Rental(customerId, carId, DateTime.Today, DateTime.Today.AddDays(2));
            typeof(Rental).GetProperty("Id")!.SetValue(rental, rentalId);

            _rentalRepositoryMock.Setup(r => r.GetByIdAsync(rentalId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(rental);

            var command = new ModifyRentalCommand
            {
                RentalId = rentalId,
                CarId = Guid.NewGuid(),
                NewStartDate = DateTime.Today.AddDays(1),
                NewEndDate = DateTime.Today.AddDays(4)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(Unit.Value));
            _rentalRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_Throw_When_RentalNotFound()
        {
            // Arrange
            _rentalRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Rental)null);

            var command = new ModifyRentalCommand
            {
                RentalId = Guid.NewGuid(),
                CarId = Guid.NewGuid(),
                NewStartDate = DateTime.Today,
                NewEndDate = DateTime.Today.AddDays(1)
            };

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Should_Throw_When_DatesAreInvalid()
        {
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();
            var rental = new Rental(customerId, carId, DateTime.Today, DateTime.Today.AddDays(2));
            typeof(Rental).GetProperty("Id")!.SetValue(rental, rentalId);

            _rentalRepositoryMock.Setup(r => r.GetByIdAsync(rentalId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(rental);

            var command = new ModifyRentalCommand
            {
                RentalId = rentalId,
                CarId = Guid.NewGuid(),
                NewStartDate = DateTime.Today.AddDays(5),
                NewEndDate = DateTime.Today.AddDays(4)
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Should_Throw_When_CarId_Is_Empty()
        {
            var rentalId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var carId = Guid.NewGuid();
            var rental = new Rental(customerId, carId, DateTime.Today, DateTime.Today.AddDays(2));
            typeof(Rental).GetProperty("Id")!.SetValue(rental, rentalId);

            _rentalRepositoryMock.Setup(r => r.GetByIdAsync(rentalId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(rental);

            var command = new ModifyRentalCommand
            {
                RentalId = rentalId,
                CarId = Guid.Empty,
                NewStartDate = DateTime.Today,
                NewEndDate = DateTime.Today.AddDays(3)
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }
    }
}
