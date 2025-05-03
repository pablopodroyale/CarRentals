using CarRental.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Rentals.Commands.CancelRental
{
    public class CancelRentalCommandHandler : IRequestHandler<CancelRentalCommand, Unit>
    {
        private readonly IRentalRepository _repository;

        public CancelRentalCommandHandler(IRentalRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(CancelRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _repository.GetByIdAsync(request.RentalId, cancellationToken);
            if (rental == null)
                throw new KeyNotFoundException("Rental not found");

            rental.Cancel();
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
