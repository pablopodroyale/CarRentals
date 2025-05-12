using CarRental.Shared.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<List<DailySummaryDto>> GetDailySummaryAsync(int days, string? location, CancellationToken cancellationToken);
        Task<List<MostUsedGroupDto>> GetMostUsedByGroupAsync(DateTime from, DateTime to, string? location, CancellationToken cancellationToken);
        Task<List<UtilizationDto>> GetUtilizationAsync(DateTime? from, DateTime? to, string? location, CancellationToken ct);
    }
}
