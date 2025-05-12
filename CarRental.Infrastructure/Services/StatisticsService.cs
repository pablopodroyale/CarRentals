using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Services
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IRentalRepository _rentalRepo;
        private readonly ICarRepository _carRepository;

        public StatisticsService(IRentalRepository rentalRepo, ICarRepository carRepository)
        {
            _rentalRepo = rentalRepo;
            _carRepository = carRepository;
        }

        public async Task<List<MostUsedGroupDto>> GetMostUsedByGroupAsync(DateTime from, DateTime to, string? location, CancellationToken cancellation)
        {
            var rentals = await _rentalRepo.GetAllAsync(null, null, from, to, location, cancellation);

            if (!string.IsNullOrEmpty(location))
            {
                rentals = rentals.Where(r => r.Car.Location == location).ToList();
            }

            var result = rentals
                .GroupBy(r => new { r.Car.Type, r.Car.Model })
                .Select(g => new MostUsedGroupDto
                {
                    Type = g.Key.Type,
                    Model = g.Key.Model,
                    TimesRented = g.Count()
                })
                .OrderByDescending(r => r.TimesRented)
                .ToList();

            return result;
        }

        public async Task<List<UtilizationDto>> GetUtilizationAsync(DateTime? from, DateTime? to, string? location, CancellationToken ct)
        {
            var allRentals = await _rentalRepo.GetAllAsync(null, null, from ?? DateTime.MinValue, to ?? DateTime.MaxValue, location, ct);

            if (!string.IsNullOrEmpty(location))
            {
                allRentals = allRentals.Where(r => r.Car.Location == location).ToList();
            }

            var cars = allRentals
                .Select(r => r.Car)
                .Distinct()
                .ToList();

            var groupedByType = allRentals
                .Where(r => !r.IsCanceled)
                .GroupBy(r => r.Car.Type)
                .Select(g =>
            {
                var totalCarsOfType = cars.Count(c => c.Type == g.Key);
                var rentedCars = g.Select(r => r.Car.Id).Distinct().Count();

                double percentage = totalCarsOfType == 0 ? 0 : (double)rentedCars / totalCarsOfType * 100;

                return new UtilizationDto
                {
                    Type = g.Key,
                    PercentageUsed = Math.Round(percentage, 2)
                };
            })
            .OrderByDescending(u => u.PercentageUsed)
            .ToList();

            return groupedByType;
        }

        public async Task<List<DailySummaryDto>> GetDailySummaryAsync(int days, string? location, CancellationToken ct)
        {
            var today = DateTime.UtcNow.Date;
            var from = today.AddDays(-days + 1);

            var rentals = await _rentalRepo.GetAllAsync(null, null, from, today, location, ct);
            var cars = await _carRepository.GetAllAsync(ct);

            if (!string.IsNullOrEmpty(location))
            {
                cars = cars.Where(c => c.Location == location).ToList();
            }

            var summaries = new List<DailySummaryDto>();

            for (int i = 0; i < days; i++)
            {
                var date = from.AddDays(i);
                var rentalsOfDay = rentals.Where(r => r.StartDate.Date <= date && r.EndDate.Date >= date).ToList();

                var activeRentalCount = rentalsOfDay.Count(r => !r.IsCanceled);
                var cancellationCount = rentalsOfDay.Count(r => r.IsCanceled);
                var usedCarIds = rentalsOfDay.Where(r => !r.IsCanceled).Select(r => r.Car.Id).Distinct();
                var unusedCars = cars.Count(c => !usedCarIds.Contains(c.Id));

                summaries.Add(new DailySummaryDto
                {
                    Date = date,
                    Rentals = activeRentalCount,
                    Cancellations = cancellationCount,
                    UnusedCars = unusedCars
                });
            }

            return summaries.OrderBy(s => s.Date).ToList();
        }
    }
}
