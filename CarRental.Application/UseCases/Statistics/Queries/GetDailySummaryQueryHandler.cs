using CarRental.Application.UseCases.Statistics.Queries;
using CarRental.Application.Interfaces;
using CarRental.Shared.DTOs.Statistic;
using MediatR;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.UseCases.Statistics.Handlers
{
    public class GetDailySummaryQueryHandler : IRequestHandler<GetDailySummaryQuery, List<DailySummaryDto>>
    {
        private readonly IStatisticsService _statisticsService;

        public GetDailySummaryQueryHandler(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<List<DailySummaryDto>> Handle(GetDailySummaryQuery request, CancellationToken cancellationToken)
        {
            return await _statisticsService.GetDailySummaryAsync(request.Days, request.Location, cancellationToken);
        }
    }
}
