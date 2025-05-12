using CarRental.Application.UseCases.Statistics.Queries;
using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Statistic;
using Moq;

namespace CarRental.Tests.Application.UseCases.Statistics
{
    [TestFixture]
    public class GetUtilizationQueryHandlerTests
    {
        private Mock<IStatisticsService> _statisticsServiceMock;
        private GetUtilizationQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _statisticsServiceMock = new Mock<IStatisticsService>();
            _handler = new GetUtilizationQueryHandler(_statisticsServiceMock.Object);
        }

        [Test]
        public async Task Should_Return_Correct_Utilization_Percentage_By_Car_Type()
        {
            // Arrange
            var expectedUtilization = new List<UtilizationDto>
            {
                new UtilizationDto { Type = "SUV", PercentageUsed = 100.0 },
                new UtilizationDto { Type = "Sedan", PercentageUsed = 50.0 }
            };

            _statisticsServiceMock
                .Setup(s => s.GetUtilizationAsync(
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUtilization);

            var query = new GetUtilizationQuery
            {
                From = DateTime.UtcNow.AddDays(-7),
                To = DateTime.UtcNow,
                Location = "Ezeiza"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(r => r.Type == "SUV" && r.PercentageUsed == 100.0), Is.True);
            Assert.That(result.Any(r => r.Type == "Sedan" && r.PercentageUsed == 50.0), Is.True);
        }
    }
}
