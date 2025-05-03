using CarRental.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases
{
    public class RegisterRentalUseCase
    {
        private readonly IRentalService _rentalService;

        public RegisterRentalUseCase(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<Guid> ExecuteAsync(Guid customerId, string carType, DateTime start, DateTime end)
        {
            return await _rentalService.RegisterRentalAsync(customerId, carType, start, end);
        }
    }
}
