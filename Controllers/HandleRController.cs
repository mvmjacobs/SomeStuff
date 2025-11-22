using Microsoft.AspNetCore.Mvc;
using SomeStuff.Application.Mediator.HandleR;
using SomeStuff.Application.Mediator.HandleR.Commands;

namespace SomeStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class HandleRController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command, CancellationToken cancellationToken)
    {
        var handler = new HandleR();
        var result = await handler.HandleAsync(command);
        return Ok(result);
    }
}
