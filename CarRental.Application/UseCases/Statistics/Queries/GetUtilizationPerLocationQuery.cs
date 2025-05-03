using CarRental.Application.DTOs.Statistic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetUtilizationPerLocationQuery : IRequest<List<UtilizationByLocationDto>>
    {
    }
}
