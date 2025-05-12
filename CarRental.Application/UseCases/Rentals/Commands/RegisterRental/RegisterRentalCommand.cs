using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommand: IRequest<Guid>
    {
        public string CustomerId { get; init; }
        public string CarType { get; init; }
        public string Model { get; set; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
