using FluentValidation;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommandValidator : AbstractValidator<RegisterRentalCommand>
    {
        public RegisterRentalCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.CarType).NotEmpty().WithMessage("Car type is required.");
            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("Start date must be before end date.");
            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.Today.AddDays(-1))
                .WithMessage("End date must be in the future.");
        }
    }
}
