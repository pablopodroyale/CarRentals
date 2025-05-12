using CarRental.Domain.Entities;
using CarRental.Shared.DTOs.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface ICarService
    {
        Task<List<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, string? carType, string? model, string location, CancellationToken cancellationToken);
        Task<List<UpcomingServiceCarDto>> GetCarsWithServiceInNextTwoWeeksAsync();

    }
}
