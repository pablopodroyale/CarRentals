using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Exceptions
{
    public class NoCarAvailableException : Exception
    {
        public NoCarAvailableException() : base("No car available for the selected dates.") { }
    }
}
