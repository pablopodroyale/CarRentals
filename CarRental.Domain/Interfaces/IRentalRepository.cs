using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

    

