using CarRental.Shared.DTOs.Rental;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.UseCases.Rentals.Query.List
{
    public class GetAllRentalsQuery : IRequest<List<RentalDto>>
    {
        public string? CustomerID { get; set; }
        public string Role { get; set; }

        public GetAllRentalsQuery(string customerID, string role) 
        {
            CustomerID = customerID;
            Role = role;
        }
    }
}
