using CarRental.Shared.DTOs.Statistic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetMostUsedByGroupQuery : IRequest<List<MostUsedGroupDto>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Location { get; set; }
    }
}
