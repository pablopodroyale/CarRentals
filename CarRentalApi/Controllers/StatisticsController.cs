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

        [HttpGet("most-rented-type")]
        public async Task<IActionResult> GetMostRentedCarType()
        {
            var result = await _mediator.Send(new GetMostRentedCarTypeQuery());
            return Ok(result);
        }

        [HttpGet("utilization-by-location")]
        public async Task<IActionResult> GetUtilizationPerLocation()
        {
            var result = await _mediator.Send(new GetUtilizationPerLocationQuery());
            return Ok(result);
        }
    }
}
