﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Shared.DTOs.Statistic
{
    public class MostUsedGroupDto
    {
        public string Type { get; set; }
        public string Model { get; set; }
        public int TimesRented { get; set; }
    }
}
