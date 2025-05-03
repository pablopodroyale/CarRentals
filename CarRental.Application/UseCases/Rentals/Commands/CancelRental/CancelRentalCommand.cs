using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Rentals.Commands.CancelRental
{
    public class CancelRentalCommand : IRequest<Unit>
    {
        public Guid RentalId { get; set; }
    }
}
