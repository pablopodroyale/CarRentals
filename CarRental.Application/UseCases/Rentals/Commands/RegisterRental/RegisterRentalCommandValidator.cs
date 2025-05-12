using FluentValidation;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommandValidator : AbstractValidator<RegisterRentalCommand>
    {
        public RegisterRentalCommandValidator()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty();

            RuleFor(x => x.CarType)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("Start date must be before end date.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");
        }
    }
}
