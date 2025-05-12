using CarRental.Domain.Entities;
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

        public async Task<List<Car>> GetAvailableCarsAsync(
                            DateTime startDate,
                            DateTime endDate,
                            string? carType,
                            string? model,
                            string? location, // <- que también puede ser opcional
                            CancellationToken cancellationToken)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(carType))
                query = query.Where(c => c.Type == carType);

            if (!string.IsNullOrEmpty(model))
                query = query.Where(c => c.Model == model);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(c => c.Location == location);

            query = query.Where(c =>
                !_context.Rentals.Any(r =>
                    r.Car.Id == c.Id &&
                    !r.IsCanceled &&
                    r.StartDate < endDate &&
                    r.EndDate > startDate
                )
            );

            return await query.ToListAsync(cancellationToken);
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
