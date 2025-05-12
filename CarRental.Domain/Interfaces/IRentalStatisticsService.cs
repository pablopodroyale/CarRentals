using CarRental.Application.DTOs.Statistic;

namespace CarRental.Domain.Interfaces
{
    public interface IRentalStatisticsService
    {
        Task<List<MostRentedCarsDto>> GetMostRentedCarsAsync();
        Task<List<UtilizationByLocationDto>> GetUtilizationPerLocationAsync();
    }
}
