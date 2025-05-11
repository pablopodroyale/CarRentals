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

        public async Task<List<RentalDto>> GetAllAsync(string customerID, string role, CancellationToken cancellationToken)
        {
            var query = _context.Rentals
                                 .Include(r => r.Customer)
                                 .Include(r => r.Car)
                                 .AsQueryable();

            if (!string.IsNullOrEmpty(customerID) && role.Equals("User"))
            {
                query = query.Where(r => r.Customer.Email == customerID);
            }

            return await query.Select(r => new RentalDto
            {
                Id = r.Id,
                CustomerId = r.Customer.Id.ToString(),
                FullName = r.Customer.FullName,
                Address = r.Customer.Address.Street,
                CarType = r.Car.Type,
                Model = r.Car.Model,
                Location = r.Car.Location,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                IsCanceled = r.IsCanceled
            }).ToListAsync(cancellationToken);

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
