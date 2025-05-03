using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Action<IServiceProvider>? SeedAction { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CarRentalDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<CarRentalDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedAction?.Invoke(scope.ServiceProvider); 
            });
        }
    }
}
