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
        Task<List<UpcomingServiceCarDto>> GetCarsWithServiceInNextTwoWeeksAsync();

    }
}
