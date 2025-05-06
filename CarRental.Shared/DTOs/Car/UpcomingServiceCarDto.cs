using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Shared.DTOs.Car
{
    public class UpcomingServiceCarDto
    {
        public string Model { get; set; }
        public string Type { get; set; }
        public DateTime NextServiceDate { get; set; }
    }
}
