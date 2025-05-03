using CarRental.Application.UseCases.Rentals.Commands.CancelRental;
using CarRental.Application.UseCases.Rentals.Commands.RegisterRental;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RentalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRentalCommand command)
    {
        var rentalId = await _mediator.Send(command);
        return Ok(new { rentalId });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelRentalCommand { RentalId = id });
        return NoContent();
    }
}
