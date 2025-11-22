using MediatR;
using Microsoft.AspNetCore.Mvc;
using SomeStuff.Application.Mediator.MediatR.Commands;
using SomeStuff.Application.Mediator.MediatR.Queries;

namespace SomeStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
    {
        var query = new GetItemsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
