using MediatR;

namespace CarRental.Application.UseCases.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommand : IRequest<Guid>
    {
        public string FullName { get; init; }
        public string Address { get; init; }
    }
}
