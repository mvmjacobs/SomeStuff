using Microsoft.AspNetCore.Mvc;
using SomeStuff.Application.Mediator.HandleR;
using SomeStuff.Application.Mediator.HandleR.Commands;

namespace SomeStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class HandleRController : ControllerBase
{
    private readonly IHandleR _handler;

    public HandleRController(IHandleR handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(command);
        return Ok(result);
    }
}
