using CarRental.Shared.DTOs.Statistic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetDailySummaryQuery : IRequest<List<DailySummaryDto>>
    {
        public int Days { get; set; }
        public string? Location { get; set; }
    }
}
