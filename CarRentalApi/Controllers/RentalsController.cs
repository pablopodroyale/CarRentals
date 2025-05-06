using CarRental.Application.UseCases.Rentals.Commands.CancelRental;
using CarRental.Application.UseCases.Rentals.Commands.ModifyRental;
using CarRental.Application.UseCases.Rentals.Commands.RegisterRental;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
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
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRentalCommand command)
    {
        var rentalId = await _mediator.Send(command);
        return Ok(new { rentalId });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelRentalCommand { RentalId = id });
        return NoContent();
    }

    [HttpPut("{rentalId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> Update(Guid rentalId, [FromBody] ModifyRentalCommand command)
    {
        command.RentalId = rentalId;
        await _mediator.Send(command);
        return NoContent();
    }
}
