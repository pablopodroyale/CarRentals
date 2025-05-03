using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;
using CarRental.Domain;
using Microsoft.AspNetCore.Identity;
using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence
{
    public class CarRentalDbContext : IdentityDbContext<ApplicationUser>
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
