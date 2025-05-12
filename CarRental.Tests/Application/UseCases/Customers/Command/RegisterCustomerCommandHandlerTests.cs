using CarRental.Application.Interfaces;
using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Services;
using CarRental.Shared.DTOs.Customer;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.Application.UseCases.Customers.Command
{
    [TestFixture]
    public class RegisterCustomerCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnCustomerId_WhenCommandIsValid()
        {
            // Arrange
            var fakeId = Guid.NewGuid();
            var userServiceMock = new Mock<ICustomerService>();

            userServiceMock.Setup(r => r.PostAsync(It.IsAny<CustomerDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(fakeId);

            var handler = new RegisterCustomerCommandHandler(userServiceMock.Object);

            var command = new RegisterCustomerCommand
            {
                Email = "Juan Pérez",
                Password = "Calle Falsa 123"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(fakeId));
            userServiceMock.Verify(r => r.PostAsync(It.IsAny<CustomerDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
