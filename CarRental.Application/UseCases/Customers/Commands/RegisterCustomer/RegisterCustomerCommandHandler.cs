using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Services;
using MediatR;

namespace CarRental.Application.UseCases.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        //private readonly ICustomerRepository _repository;
        private readonly ICustomerService _customerService;
        public RegisterCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Shared.DTOs.Customer.CustomerDto
            {
                Email = request.Email,
                Password = request.Password,
                Address = request.Address,
                FullName = request.FullName
            };

            return await _customerService.PostAsync(customer, cancellationToken);
        }
    }
}
