using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface IRentalService
    {
        Task<Guid> RegisterRentalAsync(Guid customerId, string carType, DateTime startDate, DateTime endDate);
        Task<(string type, double utilization)> GetMostRentedCarTypeAsync();

    }
}
