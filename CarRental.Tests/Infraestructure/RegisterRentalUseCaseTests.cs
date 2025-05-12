using CarRental.Application.Interfaces;
using CarRental.Application.UseCases;
using Moq;

namespace CarRental.Tests.Infraestructure
{
    [TestFixture]
    public class RegisterRentalUseCaseTests
    {
        private Mock<IRentalService> _rentalServiceMock;
        private RegisterRentalUseCase _useCase;

        [SetUp]
        public void Setup()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _useCase = new RegisterRentalUseCase(_rentalServiceMock.Object);
        }

        [Test]
        public async Task Should_RegisterRental_When_CarIsAvailable()
        {
            var customerEmail = "user@test.com";
            var expectedRentalId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerEmail, "Pickup", "Toyota Hilux", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ReturnsAsync(expectedRentalId);

            var rentalId = await _useCase.ExecuteAsync(customerEmail, "Pickup", "Toyota Hilux", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken);

            Assert.That(rentalId, Is.EqualTo(expectedRentalId));
        }

        [Test]
        public void Should_Throw_When_NoCarAvailable()
        {
            var customerEmail = "user@test.com";
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerEmail, "SUV", "Kia Sportage", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("No car available for selected dates"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerEmail, "SUV", "Kia Sportage", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("No car available for selected dates"));
        }

        [Test]
        public void Should_Throw_When_CarIsInService()
        {
            var customerEmail = "user@test.com";
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerEmail, "Sedan", "Honda Civic", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("Car in service"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerEmail, "Sedan", "Honda Civic", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("Car in service"));
        }

        [Test]
        public void Should_Throw_When_CarAlreadyRentedByAnotherCustomer()
        {
            var customerEmail = "user@test.com";
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerEmail, "Compact", "Peugeot 208", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("Car already rented"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerEmail, "Compact", "Peugeot 208", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("Car already rented"));
        }
    }
}
