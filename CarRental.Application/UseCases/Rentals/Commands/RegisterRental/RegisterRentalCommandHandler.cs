using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Services;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Commands.RegisterRental
{
    public class RegisterRentalCommandHandler : IRequestHandler<RegisterRentalCommand, Guid>
    {
        private readonly IRentalService _rentalService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly ICustomerService _customerService;

        public RegisterRentalCommandHandler(IRentalService rentalService, IEmailQueueService emailQueueService, ICustomerService customerService)
        {
            _rentalService = rentalService;
            _emailQueueService = emailQueueService;
            _customerService = customerService;
        }

        public async Task<Guid> Handle(RegisterRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalId = await _rentalService.RegisterRentalAsync(
                 request.CustomerId,
                 request.CarType,
                 request.StartDate,
                 request.EndDate, 
                 cancellationToken);

            var customerDto = await _customerService.GetAsync(request.CustomerId);
            if (customerDto != null)
            {
                var subject = "Confirmación de alquiler";
                var body = $"Hola {customerDto.Email},\n\nTu reserva fue confirmada del {request.StartDate:dd/MM/yyyy} al {request.EndDate:dd/MM/yyyy}.\nTipo de auto: {request.CarType}.\n\n¡Gracias por usar CarRental!";
                _emailQueueService.EnqueueEmail(customerDto.Email, subject, body);
            }

            return rentalId;
        }
    }
}
