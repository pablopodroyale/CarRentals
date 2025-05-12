using CarRental.Shared.DTOs.Car;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Cars.Query
{
    public class GetAvailableCarsQuery : IRequest<List<CarDto>>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string? CarType { get; init; }
        public string? Model { get; init; }
        public string? Location { get; set; }
    }
}
