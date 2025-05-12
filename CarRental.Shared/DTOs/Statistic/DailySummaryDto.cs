using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Shared.DTOs.Statistic
{
    public class DailySummaryDto
    {
        public DateTime Date { get; set; }
        public int Rentals { get; set; }
        public int Cancellations { get; set; }
        public int UnusedCars { get; set; }
    }
}
