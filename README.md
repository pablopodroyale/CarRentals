
# CarRental API

CarRental is a backend system for managing vehicle rentals, customers, and car usage statistics. It is built using ASP.NET Core 6, Entity Framework Core, MediatR, and follows Clean Architecture principles.

## Features

- Customer and vehicle management
- Rental registration, modification, and cancellation
- JWT authentication and authorization
- Statistics for most rented car types and utilization by location
- In-memory caching
- Integration and unit testing with NUnit and Moq

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server or use InMemory DB for tests

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-org/CarRentalSolution.git
   cd CarRentalSolution
   ```

2. Update database:
   ```bash
   dotnet ef database update --project CarRental.Infrastructure --startup-project CarRentalApi
   ```

## Migrations

To create a new migration:
```bash
EntityFrameworkCore\Add-Migration MigrationName -Project CarRental.Infrastructure -StartupProject CarRentalApi
```

To apply migrations:
```bash
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRentalApi
```

## Running the Application

Navigate to the API project and run:
```bash
dotnet run --project CarRentalApi
```

## Running Tests

To execute unit and integration tests:
```bash
dotnet test
```

## Generating Test Reports

To generate test reports (using NUnit):
```bash
dotnet test --logger:"trx;LogFileName=TestResults.trx"
```

You can use tools like [ReportGenerator](https://github.com/danielpalme/ReportGenerator) to convert `.trx` files to HTML reports.

---

**Author**: Pablo Ernesto Podgaiz  
**License**: MIT
