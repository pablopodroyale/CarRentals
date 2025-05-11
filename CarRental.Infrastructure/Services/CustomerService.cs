using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Identity;
using CarRental.Shared.DTOs.Customer;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerService(ICustomerRepository customerRepository, UserManager<ApplicationUser> userManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
        }
        public async Task<CustomerDto> GetAsync(string customerID)
        {
            var customer = await _customerRepository.GetAsync(customerID);
            if (customer == null)
                throw new Exception($"Customer with ID {customerID} not found.");

            return new CustomerDto
            {
                Email = customer.Email,
                Id = customer.Id
            };
        }

        public async Task<Guid> PostAsync(CustomerDto request, CancellationToken cancellationToken)
        {
            // 1. Crear usuario en Identity
            var appUser = new ApplicationUser
            {
                UserName = request?.Email,
                Email = request.Email
            };

            var identityResult = await _userManager.CreateAsync(appUser, request.Password);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Error creando usuario: {errors}");
            }

            // 2. Asignar rol por defecto (opcional)
            await _userManager.AddToRoleAsync(appUser, "User");

            // 3. Crear entidad de dominio Customer con link a ApplicationUser
            var newCustomer = new Customer
            {
                Email = request.Email,
                ApplicationUserId = appUser.Id,
                Address = new Address {
                    Street = request.Address
                },
                FullName = request.FullName
            };

            return await _customerRepository.AddAsync(newCustomer, cancellationToken);
        }
    }
}
