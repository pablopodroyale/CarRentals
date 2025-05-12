using CarRental.Domain.Entities;
using CarRental.Shared.DTOs.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface IRentalService
    {
        Task<List<RentalDto>> GetAllAsync(string customerID, string? role, DateTime? from, DateTime? to, string? location, CancellationToken cancellationToken);
        Task<Guid> RegisterRentalAsync(string customerId, string carType, string model, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task UpdateAsync(Guid rentalId, DateTime newStartDate, DateTime newEndDate, Car car, CancellationToken cancellationToken);
    }
}
