using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;
using CarRental.Domain.Rules.Rental;
using CarRental.Infrastructure.Persistence;
using CarRental.Shared.DTOs.Rental;
using Microsoft.EntityFrameworkCore;

public class RentalService : IRentalService
{
    private readonly CarRentalDbContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEnumerable<IRentalRule> _rentalRules;
    private readonly IRentalRepository _rentalRepository;

    public RentalService(CarRentalDbContext context, IEnumerable<IRentalRule> rentalRules, ICustomerRepository customerRepository, IRentalRepository rentalRepository)
    {
        _context = context;
        _rentalRules = rentalRules;
        _customerRepository = customerRepository;
        _rentalRepository = rentalRepository;
    }

    public Task<List<RentalDto>> GetAllAsync(string customerID, string role, CancellationToken cancellationToken)
    {
        return _rentalRepository.GetAllAsync(customerID, role, cancellationToken);
    }

    public async Task<Guid> RegisterRentalAsync(string customerId, string carType, string model, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(customerId);
        _context.Attach(customer);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        // Buscar autos que no tengan reservas conflictivas y no estén en servicio
        var candidateCars = await _context.Cars
            .Include(c => c.Services)
            .Where(c =>
                c.Type == carType &&
                !c.Services.Any(s => s.Date >= startDate && s.Date <= endDate) &&
                !_context.Rentals.Any(r =>
                    r.Car.Id == c.Id &&
                    !r.IsCanceled &&
                    r.EndDate.AddDays(1) >= startDate &&
                    r.StartDate <= endDate
                )
            )
            .ToListAsync();

        foreach (var car in candidateCars)
        {
            var rental = new Rental(customer, car, startDate, endDate);

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
