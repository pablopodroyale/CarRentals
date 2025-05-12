using CarRental.Domain;
using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllAsync(CancellationToken ct);
        Task<List<Car>> GetAvailableCars(DateTime startDate, DateTime endDate, string type);
        Task<Car> GetByIdAsync(Guid carId, CancellationToken cancellationToken);
    }
}
