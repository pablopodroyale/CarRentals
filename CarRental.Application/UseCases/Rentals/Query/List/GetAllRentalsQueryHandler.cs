using CarRental.Application.Interfaces;
using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Rental;
using MediatR;

namespace CarRental.Application.UseCases.Rentals.Query.List;

public class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, List<RentalDto>>
{
    private readonly IRentalService _rentalService;

    public GetAllRentalsQueryHandler(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    public async Task<List<RentalDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
    {
        return await _rentalService.GetAllAsync(request.CustomerID, request.Role, null, null, null, cancellationToken);
    }
}
