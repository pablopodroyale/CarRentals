using CarRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Rules.Rental
{
    public class CarServiceConflictRule : IRentalRule
    {
        public Task ValidateAsync(Entities.Rental rental, CancellationToken c)
        {
            var hasConflict = rental.Car.Services
                .Any(s => s.Date >= rental.StartDate && s.Date <= rental.EndDate);

            if (hasConflict)
                throw new BusinessRentalRuleException("Car has a scheduled service during the rental period.");

            return Task.CompletedTask;
        }
    }

}
