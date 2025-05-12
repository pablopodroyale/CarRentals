//using CarRental.Application.DTOs.Statistic;
//using CarRental.Application.UseCases.Statistics.Queries;
//using CarRental.Domain.Interfaces;
//using Moq;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CarRental.Tests.UseCases.Statistics
//{
//    [TestFixture]
//    public class StatisticsHandlersTests
//    {
//        private Mock<IRentalStatisticsService> _rentalStatisticsServiceMock;

//        [SetUp]
//        public void Setup()
//        {
//            _rentalStatisticsServiceMock = new Mock<IRentalStatisticsService>();
//        }

//        [Test]
//        public async Task Should_Return_Utilization_By_Location()
//        {
//            // Arrange
//            var handler = new GetUtilizationQueryHandler(_rentalStatisticsServiceMock.Object);
//            var expected = new List<UtilizationByLocationDto>
//            {
//                new UtilizationByLocationDto { Location = "Buenos Aires", Utilization = 55.2 },
//                new UtilizationByLocationDto { Location = "Córdoba", Utilization = 33.1 }
//            };

//            _rentalStatisticsServiceMock.Setup(s => s.GetUtilizationPerLocationAsync())
//                .ReturnsAsync(expected);

//            var query = new GetUtilizationQuery();

//            // Act
//            var result = await handler.Handle(query, CancellationToken.None);

//            // Assert
//            Assert.That(result.Count, Is.EqualTo(2));
//            Assert.That(result.Any(r => r.Location == "Buenos Aires" && r.Utilization == 55.2), Is.True);
//            Assert.That(result.Any(r => r.Location == "Córdoba" && r.Utilization == 33.1), Is.True);
//        }
//    }
//}
