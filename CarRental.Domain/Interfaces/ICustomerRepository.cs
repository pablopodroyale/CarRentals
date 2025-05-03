﻿using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken);
    }
}
