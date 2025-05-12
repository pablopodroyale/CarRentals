using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Statistic;
using MediatR;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetUtilizationQueryHandler : IRequestHandler<GetUtilizationQuery, List<UtilizationDto>>
    {
        private readonly IStatisticsService _statisticsService;

        public GetUtilizationQueryHandler(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<List<UtilizationDto>> Handle(GetUtilizationQuery request, CancellationToken cancellationToken)
        {
            return await _statisticsService.GetUtilizationAsync(
                request.From,
                request.To,
                request.Location,
                cancellationToken);
        }
    }

}
