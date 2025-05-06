using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetByCarIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

    

