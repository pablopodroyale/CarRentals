using CarRental.Application.UseCases.Rentals.Commands.RegisterRental;
using NUnit.Framework.Internal;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace CarRental.Tests.Application.UseCases.Rentals
{
    [TestFixture]
    public class RegisterRentalCommandValidatorTests
    {
        private RegisterRentalCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RegisterRentalCommandValidator();
        }

        [Test]
        public void Should_Fail_When_CustomerId_Is_Empty()
        {
            var command = new RegisterRentalCommand
            {
                CustomerId = "user@test.com",
                CarType = "SUV",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2)
            };

            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == nameof(command.CustomerId)));
        }

        [Test]
        public void Should_Fail_When_CarType_Is_Empty()
        {
            var command = new RegisterRentalCommand
            {
                CustomerId = "user@test.com",
                CarType = "",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2)
            };

            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == nameof(command.CarType)));
        }

        [Test]
        public void Should_Fail_When_StartDate_Is_After_EndDate()
        {
            var command = new RegisterRentalCommand
            {
                CustomerId = "user@test.com",
                CarType = "SUV",
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today
            };

            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == nameof(command.StartDate)));
        }

        [Test]
        public void Should_Fail_When_Dates_Are_Same()
        {
            var now = DateTime.Today;
            var command = new RegisterRentalCommand
            {
                CustomerId = "user@test.com",
                CarType = "SUV",
                StartDate = now,
                EndDate = now
            };

            _validator = new RegisterRentalCommandValidator();
            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Any(e => e.PropertyName == nameof(command.EndDate)));
        }

        [Test]
        public void Should_Pass_With_Valid_Command()
        {
            var command = new RegisterRentalCommand
            {
                CustomerId = "user@test.com",
                CarType = "SUV",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3)
            };

            var result = _validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}
