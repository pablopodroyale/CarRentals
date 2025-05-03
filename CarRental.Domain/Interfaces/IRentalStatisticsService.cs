using CarRental.Application.DTOs.Statistic;

namespace CarRental.Domain.Interfaces
{
    public interface IRentalStatisticsService
    {
        Task<MostRentedCarTypeDto> GetMostRentedCarTypeAsync();
        Task<List<UtilizationByLocationDto>> GetUtilizationPerLocationAsync();
    }
}
