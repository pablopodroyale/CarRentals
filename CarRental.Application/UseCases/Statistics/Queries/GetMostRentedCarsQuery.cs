using CarRental.Application.DTOs.Statistic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Statistics.Queries
{
    public class GetMostRentedCarsQuery : IRequest<List<MostRentedCarsDto>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Location { get; set; }
    }
}
