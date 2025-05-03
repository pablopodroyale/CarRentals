using CarRental.Application.Interfaces;
using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class RegisterCustomerCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnCustomerId_WhenCommandIsValid()
        {
            // Arrange
            var fakeId = Guid.NewGuid();
            var mockRepo = new Mock<ICustomerRepository>();

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(fakeId);

            var handler = new RegisterCustomerCommandHandler(mockRepo.Object);

            var command = new RegisterCustomerCommand
            {
                FullName = "Juan Pérez",
                Address = "Calle Falsa 123"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(fakeId));
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
