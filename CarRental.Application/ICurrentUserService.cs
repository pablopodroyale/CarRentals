using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Email { get; }
        Task<IList<string>> GetRolesAsync();
    }
}
