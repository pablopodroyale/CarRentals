using CarRental.Domain.Entities;
using CarRental.Shared.DTOs.Customer;

namespace CarRental.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> GetAsync(string customerID);
    }
}
