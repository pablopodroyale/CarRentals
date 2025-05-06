using CarRental.Shared.DTOs.Car;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Cars.Query
{
    public class GetUpcomingServicesQuery : IRequest<List<UpcomingServiceCarDto>> { }

}
