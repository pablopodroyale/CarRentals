using CarRental.Application.DTOs.Statistic;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetMostRentedCarsQueryHandler : IRequestHandler<GetMostRentedCarsQuery, List<MostRentedCarsDto>>
    {
        private readonly IRentalStatisticsService _service;

        public GetMostRentedCarsQueryHandler(IRentalStatisticsService service)
        {
            _service = service;
        }

        public async Task<List<MostRentedCarsDto>> Handle(GetMostRentedCarsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetMostRentedCarsAsync();
        }
    }
}
