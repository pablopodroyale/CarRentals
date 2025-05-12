# CarRental API

CarRental is a backend system for managing vehicle rentals, customers, and car usage statistics. It is built using ASP.NET Core 6, Entity Framework Core, MediatR, and follows Clean Architecture principles.

🧾 **GitHub Repo**: [pablopodroyale/CarRentals](https://github.com/pablopodroyale/CarRentals.git)

---

## ✨ Features

- Customer and vehicle management
- Rental registration, modification, and cancellation
- JWT authentication and authorization
- Statistics: most rented cars, utilization, daily summary
- In-memory caching
- Durable Functions for email confirmation
- Integration and unit testing with NUnit and Moq

---

## 🛠 Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server (for dev) or InMemory (for testing)
- Azure Storage Emulator (e.g., [Azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio))
- Docker (optional but recommended)

---

## 🚀 Setup

1. Clone the repository:

```bash
git clone https://github.com/pablopodroyale/CarRentals.git
cd CarRentals
```

2. Update the database:

```bash
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRentalApi
```

---

## 🔧 Migrations

Create a new migration:

```bash
dotnet ef migrations add MigrationName --project CarRental.Infrastructure --startup-project CarRentalApi
```

Apply existing migrations:

```bash
dotnet ef database update --project CarRental.Infrastructure --startup-project CarRentalApi
```

---

## ▶️ Running the Application

You must run **both** the API and the email function:

```bash
# In separate terminals:

dotnet run --project CarRentalApi
dotnet run --project CarRental.EmailFunctions
```

Ensure both are running for features like email confirmation to work.

---

## 🐳 Running with Docker

Use Docker Compose to run both API and email function together:

```bash
docker-compose up --build
```

> Make sure you configure your `docker-compose.yml` and `.env` if needed.

---

## 🧪 Running Tests

To run all unit and integration tests:

```bash
dotnet test
```

---

## 📊 Generating Test Coverage Reports

1. Run tests with coverage:

```bash
dotnet test CarRental.Tests/CarRental.Tests.csproj --collect:"XPlat Code Coverage"
```

2. Generate HTML report:

```bash
reportgenerator -reports:"CarRental.Tests/TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:Html
```

3. Open the report in your browser:

```bash
start coverage-report/index.html
```

> If needed, install the tool:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

---

## 🧑‍💻 Author

**Pablo Ernesto Podgaiz**  
GitHub: [@pablopodroyale](https://github.com/pablopodroyale)

---

## 📄 License

MIT