using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Tests.Infrastructure.Services
{
    [TestFixture]
    public class CarServiceTests
    {
        private CarRentalDbContext _context;
        private CarService _carService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CarRentalDbContext(options);

            _context.Cars.Add(new Car
            {
                Type = "SUV",
                Model = "Toyota",
                Location = "Buenos Aires",
                Services = new List<Service>
                {
                    // Hoy - 1 mes y 20 días → Próximo service = hoy + 10 días (✅ entra)
                    new Service { Date = DateTime.Today.AddMonths(-2).AddDays(10) }
                }
            });

            _context.SaveChanges();
            _carService = new CarService(_context);
        }

        [Test]
        public async Task Should_Return_Cars_With_Upcoming_Service()
        {
            // Act
            var result = await _carService.GetCarsWithServiceInNextTwoWeeksAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Model, Is.EqualTo("Toyota"));
        }
    }
}
