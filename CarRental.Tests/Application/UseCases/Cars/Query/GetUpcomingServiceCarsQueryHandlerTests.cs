using CarRental.Application.UseCases.Cars.Query;
using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Car;
using Moq;

namespace CarRental.Tests.Application.UseCases.Cars.Query
{
    [TestFixture]
    public class GetUpcomingServiceCarsQueryHandlerUnitTests
    {
        private Mock<ICarService> _carServiceMock;
        private GetUpcomingServiceCarsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _carServiceMock = new Mock<ICarService>();

            _handler = new GetUpcomingServiceCarsQueryHandler(_carServiceMock.Object);
        }

        [Test]
        public async Task Should_Return_UpcomingServiceCars_From_CarService()
        {
            // Arrange
            var expectedCars = new List<UpcomingServiceCarDto>
            {
                new UpcomingServiceCarDto
                {
                    Model = "Toyota RAV4",
                    Type = "SUV",
                    NextServiceDate = DateTime.Today.AddDays(10)
                }
            };

            _carServiceMock
                .Setup(cs => cs.GetCarsWithServiceInNextTwoWeeksAsync())
                .ReturnsAsync(expectedCars);

            // Act
            var result = await _handler.Handle(new GetUpcomingServicesQuery(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Model, Is.EqualTo("Toyota RAV4"));
        }

        [Test]
        public async Task Should_Return_EmptyList_When_No_Cars_Are_Upcoming_For_Service()
        {
            // Arrange
            _carServiceMock
                .Setup(cs => cs.GetCarsWithServiceInNextTwoWeeksAsync())
                .ReturnsAsync(new List<UpcomingServiceCarDto>());

            // Act
            var result = await _handler.Handle(new GetUpcomingServicesQuery(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}
