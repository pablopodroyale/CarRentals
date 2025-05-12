using CarRental.Application.DTOs.Statistic;
using CarRental.Application.UseCases.Statistics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /**
         *  Top 3 Most rented cars
        **/
        [HttpGet("most-rented")]
        public async Task<IActionResult> GetMostRented(
                 [FromQuery] DateTime from,
                 [FromQuery] DateTime to,
                 [FromQuery] string? location = null)
        {
            var query = new GetMostRentedCarsQuery
            {
                From = from,
                To = to,
                Location = location
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("most-used-by-group")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMostUsedGrouped([FromQuery] DateTime from, [FromQuery] DateTime to, string? location = null)
        {
            var result = await _mediator.Send(new GetMostUsedByGroupQuery { From = from, To = to, Location = location });
            return Ok(result);
        }

        [HttpGet("utilization")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUtilization([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? location = null)
        {
            var result = await _mediator.Send(new GetUtilizationQuery());
            return Ok(result);
        }

        [HttpGet("daily-summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDailySummary([FromQuery] int days = 7, [FromQuery] string? location = null)
        {
            var result = await _mediator.Send(new GetDailySummaryQuery
            {
                Days = days,
                Location = location
            });

            return Ok(result);
        }
    }
}
