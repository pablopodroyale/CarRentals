using CarRental.Application.UseCases.Statistics.Queries;
using CarRental.Application.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.Interfaces;

namespace CarRental.Tests.UseCases.Statistics
{
    [TestFixture]
    public class GetMostRentedCarTypeQueryHandlerTests
    {
        private Mock<IRentalStatisticsService> _rentalStatisticsServiceMock;

        [SetUp]
        public void Setup()
        {
            _rentalStatisticsServiceMock = new Mock<IRentalStatisticsService>();
        }

        [Test]
        public async Task Should_Return_MostRentedCarType()
        {
            var mockService = new Mock<IRentalStatisticsService>();
            mockService.Setup(s => s.GetMostRentedCarTypeAsync())
                       .ReturnsAsync(
                        new CarRental.Application.DTOs.Statistic.MostRentedCarTypeDto 
                       {
                           Type = "SUV",
                           UtilizationPercentage = 65.0
                       });

            var handler = new GetMostRentedCarTypeQueryHandler(mockService.Object);
            var result = await handler.Handle(new GetMostRentedCarTypeQuery(), CancellationToken.None);

            Assert.That(result.Type, Is.EqualTo("SUV"));
            Assert.That(result.UtilizationPercentage, Is.EqualTo(65.0));
        }   
    }
}
