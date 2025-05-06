using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Commands.ModifyRental
{
    public class ModifyRentalCommandHandler : IRequestHandler<ModifyRentalCommand, Unit>
    {
        private readonly IRentalService _rentalService;
        private readonly ICarRepository _carRepository;

        public ModifyRentalCommandHandler(IRentalService rentalService, ICarRepository carRepository)
        {
            _rentalService = rentalService;
            _carRepository = carRepository;
        }

        public async Task<Unit> Handle(ModifyRentalCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetByIdAsync(request.CarId, cancellationToken);
            if (car == null)
                throw new KeyNotFoundException("Car not found");

            await _rentalService.UpdateAsync(
                request.RentalId,
                request.NewStartDate,
                request.NewEndDate,
                car,
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
