﻿using CarRental.Domain.Entities;
using CarRental.Shared.DTOs.Customer;

namespace CarRental.Infrastructure.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetAsync(string customerID);
        Task<Guid> PostAsync(CustomerDto customer, CancellationToken cancellationToken);
    }
}
