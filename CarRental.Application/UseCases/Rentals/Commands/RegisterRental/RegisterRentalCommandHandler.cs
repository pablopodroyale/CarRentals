using CarRental.Application.Interfaces;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommandHandler : IRequestHandler<RegisterRentalCommand, Guid>
    {
        private readonly IRentalService _rentalService;

        public RegisterRentalCommandHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<Guid> Handle(RegisterRentalCommand request, CancellationToken cancellationToken)
        {
            return await _rentalService.RegisterRentalAsync(
                request.CustomerId,
                request.CarType,
                request.StartDate,
                request.EndDate);
        }
    }
}
