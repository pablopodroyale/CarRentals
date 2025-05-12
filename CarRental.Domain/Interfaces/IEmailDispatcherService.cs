using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface IEmailDispatcherService
    {
        Task SendConfirmationEmailAsync(string to, string subject, string body);
    }
}
