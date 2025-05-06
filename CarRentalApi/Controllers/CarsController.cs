using CarRental.Application.UseCases.Cars.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]

        [HttpGet("upcoming-service")]
        public async Task<IActionResult> GetUpcomingServiceCars()
        {
            var result = await _mediator.Send(new GetUpcomingServicesQuery());
            return Ok(result);
        }
    }
}
