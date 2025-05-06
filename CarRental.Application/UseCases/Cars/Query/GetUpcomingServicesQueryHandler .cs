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
    public class GetUpcomingServiceCarsQueryHandler : IRequestHandler<GetUpcomingServicesQuery, List<UpcomingServiceCarDto>>
    {
        private readonly ICarService _carService;

        public GetUpcomingServiceCarsQueryHandler(ICarService carService)
        {
            _carService = carService;
        }

        public Task<List<UpcomingServiceCarDto>> Handle(GetUpcomingServicesQuery request, CancellationToken cancellationToken)
        {
            return _carService.GetCarsWithServiceInNextTwoWeeksAsync();
        }
    }

}
