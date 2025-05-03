using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.Statistic
{
    public class UtilizationByLocationDto
    {
        public string Location { get; set; }
        public double Utilization { get; set; }
    }
}
