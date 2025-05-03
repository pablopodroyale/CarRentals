using CarRental.Application.UseCases.Rentals.Commands.RegisterRental;
using NUnit.Framework;

namespace CarRental.Tests.Application.UseCases.Rentals
{
    [TestFixture]
    public class RegisterRentalCommandValidatorTests
    {
        [Test]
        public void Should_Fail_When_CustomerId_Is_Empty()
        {
            var validator = new RegisterRentalCommandValidator();

            var command = new RegisterRentalCommand
            {
                CustomerId = Guid.Empty,
                CarType = "SUV",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2)
            };

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "CustomerId"));
        }

        [Test]
        public void Should_Fail_When_CarType_Is_Empty()
        {
            var validator = new RegisterRentalCommandValidator();

            var command = new RegisterRentalCommand
            {
                CustomerId = Guid.NewGuid(),
                CarType = "",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2)
            };

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "CarType"));
        }

        [Test]
        public void Should_Fail_When_Dates_Are_Invalid()
        {
            var validator = new RegisterRentalCommandValidator();

            var command = new RegisterRentalCommand
            {
                CustomerId = Guid.NewGuid(),
                CarType = "SUV",
                StartDate = DateTime.Today.AddDays(3),
                EndDate = DateTime.Today
            };

            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == "StartDate" || e.PropertyName == "EndDate"));
        }

        [Test]
        public void Should_Pass_With_Valid_Data()
        {
            var validator = new RegisterRentalCommandValidator();

            var command = new RegisterRentalCommand
            {
                CustomerId = Guid.NewGuid(),
                CarType = "SUV",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3)
            };

            var result = validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}
