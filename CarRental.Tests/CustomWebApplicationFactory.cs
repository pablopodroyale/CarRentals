using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

            // Crear usuario por defecto para autenticación
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var testUser = new ApplicationUser { UserName = "testuser@example.com", Email = "testuser@example.com" };
            userManager.CreateAsync(testUser, "Password123!").Wait();

            SeedAction?.Invoke(scope.ServiceProvider);
        });
    }

    // Client con token
    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();
        var loginRequest = new
        {
            email = "testuser@example.com",
            password = "Password123!"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
        var response = client.PostAsync("/api/auth/login", content).Result;
        response.EnsureSuccessStatusCode();

        var json = response.Content.ReadAsStringAsync().Result;
        var token = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
