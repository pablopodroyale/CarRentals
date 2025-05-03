using CarRental.Application.Interfaces;
using CarRental.Domain;
using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure
{
    public class RentalService : IRentalService
    {
        private readonly CarRentalDbContext _context;

        public RentalService(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> RegisterRentalAsync(Guid customerId, string carType, DateTime start, DateTime end)
        {
            var conflictingCarIds = await _context.Rentals
                .Where(r => r.EndDate >= start && r.StartDate <= end)
                .Select(r => r.CarId)
                .ToListAsync();

            var availableCars = await _context.Cars
                .Where(c => c.Type == carType && !conflictingCarIds.Contains(c.Id))
                .ToListAsync();

            var car = availableCars.FirstOrDefault();
            if (car == null) throw new NoCarAvailableException();

            var rental = new Rental(customerId, car.Id, start, end);

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return rental.Id;
        }

        public async Task<(string type, double utilization)> GetMostRentedCarTypeAsync()
        {
            var totalRentals = await _context.Rentals.CountAsync();
            if (totalRentals == 0)
                return ("N/A", 0);

            var top = await _context.Rentals
                .Join(_context.Cars, r => r.CarId, c => c.Id, (r, c) => c.Type)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .FirstOrDefaultAsync();

            return (top.Type, (double)top.Count / totalRentals * 100);
        }

    }
}
