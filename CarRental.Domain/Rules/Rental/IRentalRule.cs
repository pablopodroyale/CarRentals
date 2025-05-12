using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Rules.Rental
{
    public interface IRentalRule
    {
        Task ValidateAsync(Entities.Rental rental, CancellationToken cancellation);

    }
}
