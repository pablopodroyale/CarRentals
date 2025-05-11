using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Car;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Cars.Query
{
    public class GetAvailableCarsQueryHandler : IRequestHandler<GetAvailableCarsQuery, List<CarDto>>
    {
        private readonly ICarService _carService;

        public GetAvailableCarsQueryHandler(ICarService carService)
        {
            _carService = carService;
        }

        public async Task<List<CarDto>> Handle(GetAvailableCarsQuery request, CancellationToken cancellationToken)
        {
            List<Car> cars = await _carService.GetAvailableCarsAsync(
                request.StartDate,
                request.EndDate,
                request.CarType,
                request.Model,
                request.Location,
                cancellationToken
            );

            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Type = c.Type,
                Model = c.Model,
                Location = c.Location
            }).ToList();
        }
    }
}
