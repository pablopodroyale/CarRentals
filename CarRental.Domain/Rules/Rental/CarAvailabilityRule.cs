using CarRental.Application.Interfaces;
using CarRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Rules.Rental
{
    public class CarAvailabilityRule : IRentalRule
    {
        private readonly ICarRepository _carRepo;
        private readonly IRentalRepository _rentalRepo;

        public CarAvailabilityRule(ICarRepository carRepo, IRentalRepository rentalRepo)
        {
            _carRepo = carRepo;
            _rentalRepo = rentalRepo;
        }

        public async Task ValidateAsync(Entities.Rental rental, CancellationToken cancellationToken)
        {
            List<Entities.Rental> rentals = await _rentalRepo.GetByCarIdAsync(rental.Car.Id, cancellationToken);

            if (rentals.Any(r => !r.IsCanceled && r.EndDate.AddDays(1) >= rental.StartDate && r.StartDate <= rental.EndDate))
                throw new BusinessRentalRuleException("Car is already rented in the selected period.");
        }
    }

}
