using CarRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Rules.Rental
{
    public class ValidDateRangeRule : IRentalRule
    {
        public Task ValidateAsync(Entities.Rental rental, CancellationToken c)
        {
            if (rental.EndDate <= rental.StartDate)
                throw new BusinessRentalRuleException("End date must be after start date.");
            return Task.CompletedTask;
        }
    }

}
