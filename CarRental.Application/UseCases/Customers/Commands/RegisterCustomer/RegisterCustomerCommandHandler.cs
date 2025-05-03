using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.UseCases.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;

        public RegisterCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                FullName = request.FullName,
                Address = request.Address
            };

            return await _repository.AddAsync(customer, cancellationToken);
        }
    }
}
