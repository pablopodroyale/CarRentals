using CarRental.Application.DTOs.Statistic;
using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetUtilizationPerLocationQueryHandler : IRequestHandler<GetUtilizationPerLocationQuery, List<UtilizationByLocationDto>>
    {
        private readonly IRentalStatisticsService _statisticsService;
        

        public GetUtilizationPerLocationQueryHandler(IRentalStatisticsService service)
        {
            _statisticsService = service;
        }

        public async Task<List<UtilizationByLocationDto>> Handle(GetUtilizationPerLocationQuery request, CancellationToken cancellationToken)
        {
            var data = await _statisticsService.GetUtilizationPerLocationAsync();
            return data;
        }
    }
}
