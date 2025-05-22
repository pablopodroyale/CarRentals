using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Services;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommandHandler : IRequestHandler<RegisterRentalCommand, Guid>
    {
        private readonly IRentalService _rentalService;
        private readonly ICustomerService _customerService;
        private readonly IEmailDispatcherService _emailDispatcher;

        public RegisterRentalCommandHandler(
            IRentalService rentalService,
            ICustomerService customerService,
            IEmailDispatcherService emailDispatcherService)
        {
            _rentalService = rentalService;
            _customerService = customerService;
            _emailDispatcher = emailDispatcherService;
        }

        public async Task<Guid> Handle(RegisterRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalId = await _rentalService.RegisterRentalAsync(
                request.CustomerId,
                request.CarType,
                request.Model,
                request.StartDate,
                request.EndDate,
                request.Location,
                cancellationToken);

            var customerDto = await _customerService.GetAsync(request.CustomerId);
            if (customerDto != null)
            {
                var subject = "Confirmación de alquiler";
                var body = $"Hola {customerDto.Email},\n\nTu reserva fue confirmada del {request.StartDate:dd/MM/yyyy} al {request.EndDate:dd/MM/yyyy}.\nTipo de auto: {request.CarType}.\n\n¡Gracias por usar CarRental!";

                await _emailDispatcher.SendConfirmationEmailAsync(customerDto.Email, subject, body);
            }

            return rentalId;
        }
    }
}
