using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CarRentalDbContext _context;

        public CustomerRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            customer.Id = Guid.NewGuid();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }

        public async Task<Customer> GetAsync(string customerID)
        {
            return await _context.Customers.AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Email == customerID);
        }
    }
}
