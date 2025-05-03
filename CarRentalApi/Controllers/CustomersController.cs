using CarRental.Application.UseCases.Customers.Commands.RegisterCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCustomerCommand command)
    {
        var customerId = await _mediator.Send(command);
        return Ok(new { customerId });
    }
}
