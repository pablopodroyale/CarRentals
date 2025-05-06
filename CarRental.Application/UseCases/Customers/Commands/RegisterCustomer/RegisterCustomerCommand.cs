using MediatR;

namespace CarRental.Application.UseCases.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommand : IRequest<Guid>
    {
        public string FullName { get; set; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string Address { get; set; }
    }
}
