using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Shared.DTOs.Statistic
{
    public class UtilizationDto
    {
        public string Type { get; set; } = "";
        public double PercentageUsed { get; set; }
    }
}
