using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Statistic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetMostUsedByGroupQueryHandler : IRequestHandler<GetMostUsedByGroupQuery, List<MostUsedGroupDto>>
    {
        private readonly IStatisticsService _statisticsService;

        public GetMostUsedByGroupQueryHandler(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<List<MostUsedGroupDto>> Handle(GetMostUsedByGroupQuery request, CancellationToken cancellationToken)
        {
            return await _statisticsService.GetMostUsedByGroupAsync(request.From, request.To, request.Location, cancellationToken);
        }
    }

}
