using CarRental.Domain.Entities;
using CarRental.Shared.DTOs.Rental;

namespace CarRental.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetAllAsync(string? customerID, string? role, DateTime? from, DateTime? to, string? location, CancellationToken cancellationToken);
        Task<List<Rental>> GetByCarIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

    

