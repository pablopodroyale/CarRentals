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
        private Mock<IRentalService> _rentalServiceMock;
        private Mock<ICarRepository> _carRepositoryMock;
        private ModifyRentalCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _carRepositoryMock = new Mock<ICarRepository>();
            _handler = new ModifyRentalCommandHandler(_rentalServiceMock.Object, _carRepositoryMock.Object);
        }

        [Test]
        public async Task Should_ModifyRental_When_Valid()
        {
            var rentalId = Guid.NewGuid();
            var car = new Car
            {
                Id = Guid.NewGuid(),
                Type = "SUV",
                Model = "RAV4",
                Location = "Buenos Aires"
            };

            _carRepositoryMock.Setup(c => c.GetByIdAsync(car.Id, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(car);

            var command = new ModifyRentalCommand
            {
                RentalId = rentalId,
                CarId = car.Id,
                NewStartDate = DateTime.Today.AddDays(1),
                NewEndDate = DateTime.Today.AddDays(4)
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.EqualTo(Unit.Value));
            _rentalServiceMock.Verify(s =>
                s.UpdateAsync(rentalId, command.NewStartDate, command.NewEndDate, car, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_Throw_When_CarNotFound()
        {
            _carRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Car)null!);

            var command = new ModifyRentalCommand
            {
                RentalId = Guid.NewGuid(),
                CarId = Guid.NewGuid(),
                NewStartDate = DateTime.Today,
                NewEndDate = DateTime.Today.AddDays(1)
            };

            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
