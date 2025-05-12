using CarRental.Application.Interfaces;
using CarRental.Domain;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CarRental.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentalDbContext _context;

        public CarRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Cars
           .Include(c => c.Services) 
           .ToListAsync(cancellationToken);
        }

        public async Task<List<Car>> GetAvailableCars(DateTime start, DateTime end, string type)
        {
            var conflictingCarIds = _context.Rentals
              .Where(r => r.EndDate >= start && r.StartDate <= end)
              .Select(r => r.Car.Id)
              .ToList();

            return await _context.Cars
                .Where(c => c.Type == type && !conflictingCarIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Car> GetByIdAsync(Guid carId, CancellationToken cancellationToken)
        {
            return await _context.Cars?
               .Include(c => c.Services)
               .FirstOrDefaultAsync(c => c.Id == carId, cancellationToken);
        }
    }
}
