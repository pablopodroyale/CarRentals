using MediatR;
using System;

namespace CarRental.Application.UseCases.Rentals.Commands.ModifyRental
{
    public class ModifyRentalCommand : IRequest<Unit>
    {
        public Guid RentalId { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public string? NewCarType { get; set; }
        public Guid CarId { get; set; }
    }
}
