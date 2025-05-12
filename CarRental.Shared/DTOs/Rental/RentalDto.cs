using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Shared.DTOs.Rental
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string CarType { get; set; } = default!;
        public Guid CarId { get; set; } = default!;
        public string Model { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCanceled { get; set; }
    }

}
