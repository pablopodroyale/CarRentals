using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Application.Interfaces;
using CarRental.Application.UseCases;
using Moq;
using NUnit.Framework;

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
            var customerId = Guid.NewGuid();
            var expectedRentalId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "SUV", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ReturnsAsync(expectedRentalId);

            var rentalId = await _useCase.ExecuteAsync(customerId, "SUV", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken);

            Assert.That(rentalId, Is.EqualTo(expectedRentalId));
        }

        [Test]
        public void Should_Throw_When_NoCarAvailable()
        {
            var customerId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "SUV", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("No car available for selected dates"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "SUV", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("No car available for selected dates"));
        }

        [Test]
        public void Should_Throw_When_CarIsInService()
        {
            var customerId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "Sedan", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("Car in service"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "Sedan", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("Car in service"));
        }

        [Test]
        public void Should_Throw_When_CarAlreadyRentedByAnotherCustomer()
        {
            var customerId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "Compact", It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                .ThrowsAsync(new Exception("Car already rented"));

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "Compact", DateTime.Today, DateTime.Today.AddDays(3), cancellationToken));

            Assert.That(ex!.Message, Is.EqualTo("Car already rented"));
        }
    }
}
