using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Rules.Rental;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class RentalService : IRentalService
{
    private readonly CarRentalDbContext _context;
    private readonly IEnumerable<IRentalRule> _rentalRules;

    public RentalService(CarRentalDbContext context, IEnumerable<IRentalRule> rentalRules)
    {
        _context = context;
        _rentalRules = rentalRules;
    }

    public async Task<Guid> RegisterRentalAsync(Guid customerId, string carType, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        // Buscar autos que no tengan reservas conflictivas y no estén en servicio
        var candidateCars = await _context.Cars
            .Include(c => c.Services)
            .Where(c =>
                c.Type == carType &&
                !c.Services.Any(s => s.Date >= start && s.Date <= end) &&
                !_context.Rentals.Any(r =>
                    r.Car.Id == c.Id &&
                    !r.IsCanceled &&
                    r.EndDate.AddDays(1) >= start &&
                    r.StartDate <= end
                )
            )
            .ToListAsync();

        foreach (var car in candidateCars)
        {
            var rental = new Rental(customer, car, start, end);

            try
            {
                foreach (var rule in _rentalRules)
                    await rule.ValidateAsync(rental, cancellationToken); // o pasar cancellationToken si llega como parámetro

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();
                return rental.Id;
            }
            catch (BusinessRentalRuleException)
            {
                // skip car, try next
            }
        }

        throw new NoCarAvailableException();
    }

    public async Task UpdateAsync(Guid rentalId, DateTime newStartDate, DateTime newEndDate, Car newCar, CancellationToken cancellationToken)
    {
        var rental = await _context.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .FirstOrDefaultAsync(r => r.Id == rentalId, cancellationToken);

        if (rental == null)
            throw new KeyNotFoundException("Rental not found");

        // Simula los cambios para validación
        var modifiedRental = new Rental(rental.Customer, newCar, newStartDate, newEndDate);

        foreach (var rule in _rentalRules)
            await rule.ValidateAsync(modifiedRental, cancellationToken);

        // Aplicar cambios reales si todas las validaciones pasan
        rental.ModifyDates(newStartDate, newEndDate);
        rental.ChangeCar(newCar);

        await _context.SaveChangesAsync(cancellationToken);
    }

}
