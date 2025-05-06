using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using CarRental.Application.Interfaces;
using CarRental.Shared.DTOs.Customer;
using Moq;
using NUnit.Framework;
using CarRental.Infrastructure.Services;

namespace CarRental.Tests.Application.UseCases.Customers.Commands
{
    [TestFixture]
    public class RegisterCustomerCommandHandlerTests
    {
        private Mock<ICustomerService> _mockCustomerService;
        private RegisterCustomerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _handler = new RegisterCustomerCommandHandler(_mockCustomerService.Object);
        }

        [Test]
        public async Task Should_Register_Customer_When_Valid()
        {
            // Arrange
            var command = new RegisterCustomerCommand
            {
                Email = "test@example.com",
                Password = "StrongPass123!"
            };

            var expectedCustomerId = Guid.NewGuid();

            _mockCustomerService
                .Setup(s => s.PostAsync(It.Is<CustomerDto>(dto =>
                        dto.Email == command.Email && dto.Password == command.Password),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCustomerId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCustomerId));
            _mockCustomerService.Verify(s => s.PostAsync(It.IsAny<CustomerDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_Throw_Exception_When_Service_Fails()
        {
            // Arrange
            var command = new RegisterCustomerCommand
            {
                Email = "fail@example.com",
                Password = "BadPass!"
            };

            _mockCustomerService
                .Setup(s => s.PostAsync(It.IsAny<CustomerDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Failed to create user"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(ex!.Message, Is.EqualTo("Failed to create user"));
        }
    }
}
