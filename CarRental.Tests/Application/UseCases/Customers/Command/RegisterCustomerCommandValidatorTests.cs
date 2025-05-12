using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using NUnit.Framework;

namespace CarRental.Tests.Application.UseCases.Customers.Command
{
    [TestFixture]
    public class RegisterCustomerCommandValidatorTests
    {
        [Test]
        public void Should_Fail_When_Email_Is_Empty()
        {
            // Arrange
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                Email = "",
                Password = "User123!"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Email"));
        }

        [Test]
        public void Should_Fail_When_Password_Is_Empty()
        {
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                Email = "test@test.com",
                Password = ""
            };

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Password"));
        }

        [Test]
        public void Should_Pass_When_Valid_Data()
        {
            var validator = new RegisterCustomerCommandValidator();

            var command = new RegisterCustomerCommand
            {
                Email = "test@test.com",
                Password = "User123!"
            };

            var result = validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}
