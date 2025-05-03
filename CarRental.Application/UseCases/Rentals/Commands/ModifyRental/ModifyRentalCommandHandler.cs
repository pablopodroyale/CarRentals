using CarRental.Application.Interfaces;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Commands.ModifyRental
{
    public class ModifyRentalCommandHandler : IRequestHandler<ModifyRentalCommand, Unit>
    {
        private readonly IRentalRepository _repository;
        private readonly ICarRepository _carRepository;

        public ModifyRentalCommandHandler(IRentalRepository repository, ICarRepository carRepository)
        {
            _repository = repository;
            _carRepository = carRepository;
        }

        public async Task<Unit> Handle(ModifyRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _repository.GetByIdAsync(request.RentalId, cancellationToken);
            if (rental == null)
                throw new KeyNotFoundException("Rental not found");

            rental.ModifyDates(request.NewStartDate, request.NewEndDate);
            rental.ChangeCar(request.CarId);

            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
