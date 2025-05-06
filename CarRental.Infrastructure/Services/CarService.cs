using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Shared.DTOs.Car;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Services
{
    public class CarService : ICarService
    {
        private readonly CarRentalDbContext _context;

        public CarService(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<List<UpcomingServiceCarDto>> GetCarsWithServiceInNextTwoWeeksAsync()
        {
            var today = DateTime.Today;
            var threshold = today.AddDays(14);

            return await _context.Cars
                .Include(c => c.Services)
                .Select(c => new
                {
                    c.Type,
                    c.Model,
                    LastServiceDate = c.Services
                        .OrderByDescending(s => s.Date)
                        .Select(s => s.Date)
                        .FirstOrDefault()
                })
                .Select(x => new
                {
                    x.Type,
                    x.Model,
                    NextServiceDate = x.LastServiceDate != default
                        ? x.LastServiceDate.AddMonths(2)
                        : (DateTime?)null
                })
                .Where(x => x.NextServiceDate != null &&
                            x.NextServiceDate.Value >= today &&
                            x.NextServiceDate.Value <= threshold)
                .Select(x => new UpcomingServiceCarDto
                {
                    Type = x.Type,
                    Model = x.Model,
                    NextServiceDate = x.NextServiceDate.Value
                })
                .ToListAsync();
        }
    }

}
