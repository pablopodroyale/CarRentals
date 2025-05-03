using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using NUnit.Framework;

namespace CarRental.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class RegisterCustomerCommandValidatorTests
    {
        [Test]
        public void Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                FullName = "",
                Address = "123 Calle Falsa"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "FullName"));
        }

        [Test]
        public void Should_Fail_When_Address_Is_Empty()
        {
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                FullName = "Juan Pérez",
                Address = ""
            };

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Address"));
        }

        [Test]
        public void Should_Pass_When_Valid_Data()
        {
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                FullName = "Juan Pérez",
                Address = "Calle Siempre Viva 742"
            };

            var result = validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}
