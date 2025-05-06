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
        builder.UseEnvironment("Testing");
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

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Crear roles si no existen
            if (!roleManager.RoleExistsAsync("User").Result)
                roleManager.CreateAsync(new IdentityRole("User")).Wait();

            if (!roleManager.RoleExistsAsync("Admin").Result)
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();

            // Crear usuario con rol User
            var user = new ApplicationUser
            {
                UserName = "user@test.com",
                Email = "user@test.com"
            };

            userManager.CreateAsync(user, "Password123!").Wait();
            userManager.AddToRoleAsync(user, "User").Wait();

            // Crear usuario con rol Admin
            var admin = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com"
            };
            userManager.CreateAsync(admin, "Password123!").Wait();
            userManager.AddToRoleAsync(admin, "Admin").Wait();

            SeedAction?.Invoke(scope.ServiceProvider);
        });
    }

    public HttpClient CreateAuthenticatedClient(string email, string password)
    {
        var client = CreateClient();
        var loginRequest = new
        {
            email,
            password
        };

        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
        var response = client.PostAsync("/api/auth/login", content).Result;
        response.EnsureSuccessStatusCode();

        var json = response.Content.ReadAsStringAsync().Result;
        var token = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    // Accesos rápidos
    public HttpClient CreateUserClient() =>
        CreateAuthenticatedClient("user@test.com", "Password123!");

    public HttpClient CreateAdminClient() =>
        CreateAuthenticatedClient("admin@test.com", "Password123!");
}
