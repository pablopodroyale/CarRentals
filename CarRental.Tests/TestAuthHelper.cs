using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

public static class TestAuthHelper
{
    public static async Task AuthenticateAsync(HttpClient client, IServiceProvider services)
    {
        using var scope = services.CreateScope(); // ✔ Crear scope

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var testUser = new ApplicationUser { UserName = "testuser@example.com", Email = "testuser@example.com" };
        var result = await userManager.CreateAsync(testUser, "Test@123");

        if (!result.Succeeded)
            throw new Exception("Failed to create test user.");

        // Crear token manualmente o llamar a tu endpoint de login
        // Por ejemplo, usando login API:

        var loginPayload = new
        {
            email = "testuser@example.com",
            password = "Test@123"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
