using CarRental.Domain.Exceptions;

namespace CarRental.Domain.Rules.Rental
{
    public class StartDateInFutureRule : IRentalRule
    {
        public StartDateInFutureRule()
        {
        }

        public Task ValidateAsync(Entities.Rental rental, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;

            if (rental.StartDate.Date < today)
                throw new BusinessRentalRuleException("Start date cannot be in the past.");

            return Task.CompletedTask;
        }
    }
}
