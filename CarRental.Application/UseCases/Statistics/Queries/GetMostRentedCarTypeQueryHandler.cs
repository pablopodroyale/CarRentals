using CarRental.Application.DTOs.Statistic;
using CarRental.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetMostRentedCarTypeQueryHandler : IRequestHandler<GetMostRentedCarTypeQuery, MostRentedCarTypeDto>
    {
        private readonly IRentalStatisticsService _service;

        public GetMostRentedCarTypeQueryHandler(IRentalStatisticsService service)
        {
            _service = service;
        }

        public async Task<MostRentedCarTypeDto> Handle(GetMostRentedCarTypeQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetMostRentedCarTypeAsync();
        }
    }
}
