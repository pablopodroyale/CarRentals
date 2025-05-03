using System;
using System.Collections.Generic;
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
            // Arrange
            var customerId = Guid.NewGuid();
            var expectedRentalId = Guid.NewGuid();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "SUV", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(expectedRentalId);

            // Act
            var rentalId = await _useCase.ExecuteAsync(customerId, "SUV", DateTime.Today, DateTime.Today.AddDays(3));

            // Assert
            Assert.That(rentalId, Is.EqualTo(expectedRentalId));
        }

        [Test]
        public void Should_Throw_When_NoCarAvailable()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "SUV", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("No car available for selected dates"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "SUV", DateTime.Today, DateTime.Today.AddDays(3)));

            Assert.That(ex.Message, Is.EqualTo("No car available for selected dates"));
        }

        [Test]
        public void Should_Throw_When_CarIsInService()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "Sedan", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Car in service"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "Sedan", DateTime.Today, DateTime.Today.AddDays(3)));

            Assert.That(ex.Message, Is.EqualTo("Car in service"));
        }

        [Test]
        public void Should_Throw_When_CarAlreadyRentedByAnotherCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _rentalServiceMock
                .Setup(s => s.RegisterRentalAsync(customerId, "Compact", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Car already rented"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _useCase.ExecuteAsync(customerId, "Compact", DateTime.Today, DateTime.Today.AddDays(3)));

            Assert.That(ex.Message, Is.EqualTo("Car already rented"));
        }
    }
}
