using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using CarRental.Shared.DTOs.Rental;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System.Threading;

namespace CarRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly CarRentalDbContext _context;

        public RentalRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetAllAsync(
          string? customerID,
          string? role,
          DateTime? from,
          DateTime? to,
          string? location,
          CancellationToken cancellationToken)
        {
            var query = _context.Rentals
                                .Include(r => r.Customer)
                                .Include(r => r.Customer.Address)
                                .Include(r => r.Car)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(customerID) && role == "User")
            {
                query = query.Where(r => r.Customer.Email == customerID);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(r => r.Car.Location == location);
            }

            if (from.HasValue)
            {
                query = query.Where(r => r.EndDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(r => r.StartDate <= to.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<Rental>> GetByCarIdAsync(Guid carId, CancellationToken cancellationToken)
        {
            return await _context.Rentals.Where(x => x.Car.Id == carId)
                                            .Include(x => x.Car)
                                            .Include(x => x.Customer)
                                            .ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
