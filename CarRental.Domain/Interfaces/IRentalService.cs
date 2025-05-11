using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface IRentalService
    {
        Task<Guid> RegisterRentalAsync(string customerId, string carType, string model, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task UpdateAsync(Guid rentalId, DateTime newStartDate, DateTime newEndDate, Car car, CancellationToken cancellationToken);
    }
}
