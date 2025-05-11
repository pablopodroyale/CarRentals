using CarRental.API.Middlewares;
using CarRental.Application.Interfaces;
using CarRental.Application.UseCases;
using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using CarRental.Application.UseCases.Rentals.Commands.RegisterRental;
using CarRental.Application.Validators.Auth;
using CarRental.Domain.Interfaces;
using CarRental.Domain.Rules.Rental;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// --- Servicios principales ---
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // tu app Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // opcional si usás auth con cookies/token
    });
});

builder.Services.AddEndpointsApiExplorer();

// --- Swagger + JWT Bearer Auth ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarRental API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido del token JWT. Ej: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- EF Core ---
builder.Services.AddDbContext<CarRentalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("CarRental.Infrastructure")
    ));

// --- Identity + JWT ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CarRentalDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// --- MediatR ---
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<RegisterRentalCommand>());
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<RegisterCustomerCommand>());

// --- Repositorios y servicios ---
builder.Services.AddScoped<RegisterRentalUseCase>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IRentalStatisticsService, RentalStatisticsService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRentalRule, CarAvailabilityRule>();
builder.Services.AddScoped<IRentalRule, CarServiceConflictRule>();
builder.Services.AddScoped<IRentalRule, ValidDateRangeRule>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

// --- Email + Durable Functions ---
builder.Services.AddHttpClient(); // para DurableEmailDispatcherService
builder.Services.AddScoped<IEmailDispatcherService, DurableEmailDispatcherService>();
builder.Services.AddMemoryCache();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// --- Middlewares ---
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// --- Seed inicial de roles y usuarios ---
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
    var retry = 10;
    while (retry > 0)
    {
        try
        {
            context.Database.Migrate();
            await DbInitializer.SeedRolesAndUsersAsync(scope.ServiceProvider);
            break;
        }
        catch (Exception ex)
        {
            retry--;
            Console.WriteLine($" Esperando que SQL Server esté listo... Retries left: {retry}. Error: {ex.Message}");
            await Task.Delay(3000);
        }
    }
}

app.Run();

// Necesario para tests
public partial class Program { }
