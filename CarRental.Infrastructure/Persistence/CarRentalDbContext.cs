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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            builder.Entity<Customer>()
                .Property(c => c.ApplicationUserId)
                .IsRequired();

             //Si querés agregar una relación de navegación en el futuro:
             builder.Entity<Customer>()
                 .HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(c => c.ApplicationUserId);
        }
    }
}
