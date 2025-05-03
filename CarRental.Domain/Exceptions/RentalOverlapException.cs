using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Exceptions
{
    public class RentalOverlapException : Exception
    {
        public RentalOverlapException() : base("The rental overlaps with an existing one.") { }
    }
}
