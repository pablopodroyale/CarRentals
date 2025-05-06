using CarRental.Application.DTOs.Statistic;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class RentalStatisticsService : IRentalStatisticsService
{
    private readonly CarRentalDbContext _context;
    private readonly IMemoryCache _cache;

    public RentalStatisticsService(CarRentalDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<MostRentedCarTypeDto> GetMostRentedCarTypeAsync()
    {
        return await _cache.GetOrCreateAsync("MostRentedType", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            var total = await _context.Rentals.CountAsync();
            if (total == 0)
            {
                return new MostRentedCarTypeDto
                {
                    Type = "N/A",
                    UtilizationPercentage = 0
                };
            }

            var top = await _context.Rentals
                .Join(_context.Cars, r => r.Car.Id, c => c.Id, (r, c) => c.Type)
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .FirstOrDefaultAsync();

            return new MostRentedCarTypeDto
            {
                Type = top.Type,
                UtilizationPercentage = Math.Round((double)top.Count / total * 100, 2)
            };
        });
    }

    public async Task<List<UtilizationByLocationDto>> GetUtilizationPerLocationAsync()
    {
        return await _cache.GetOrCreateAsync<List<UtilizationByLocationDto>>("UtilizationPerLocation", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            var rentals = await _context.Rentals
                .Join(_context.Cars, r => r.Car.Id, c => c.Id, (r, c) => new { c.Location })
                .GroupBy(x => x.Location)
                .Select(g => new { Location = g.Key, Count = g.Count() })
                .ToListAsync();

            var total = rentals.Sum(x => x.Count);

            return rentals.Select(r => new UtilizationByLocationDto
            {
                Location = r.Location,
                Utilization = total == 0 ? 0 : Math.Round((double)r.Count / total * 100, 2)
            }).ToList();
        });
    }
}
