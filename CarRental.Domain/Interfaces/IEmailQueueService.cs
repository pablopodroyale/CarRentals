using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface IEmailQueueService
    {
        void EnqueueEmail(string to, string subject, string body);
    }
}
