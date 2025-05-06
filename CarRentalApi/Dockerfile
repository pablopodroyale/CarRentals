# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarRentalApi/CarRentalApi.csproj", "CarRentalApi/"]
COPY ["CarRental.Application/CarRental.Application.csproj", "CarRental.Application/"]
COPY ["CarRental.Domain/CarRental.Domain.csproj", "CarRental.Domain/"]
COPY ["CarRental.Shared/CarRental.Shared.csproj", "CarRental.Shared/"]
COPY ["CarRental.Infrastructure/CarRental.Infrastructure.csproj", "CarRental.Infrastructure/"]
RUN dotnet restore "./CarRentalApi/CarRentalApi.csproj"
COPY . .
WORKDIR "/src/CarRentalApi"
RUN dotnet build "./CarRentalApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarRentalApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalApi.dll"]