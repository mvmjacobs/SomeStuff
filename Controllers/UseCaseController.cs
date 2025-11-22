using Microsoft.AspNetCore.Mvc;
using SomeStuff.Application.UseCases.CreateItem;
using SomeStuff.Application.UseCases.GetItems;

namespace SomeStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class UseCaseController : ControllerBase
{
    private readonly IGetItemsUseCase _getItemsUseCase;
    private readonly ICreateItemUseCase _createItemUseCase;

    public UseCaseController(
        IGetItemsUseCase getItemsUseCase,
        ICreateItemUseCase createItemUseCase)
    {
        _getItemsUseCase = getItemsUseCase;
        _createItemUseCase = createItemUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _getItemsUseCase.ExecuteAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] object request, CancellationToken cancellationToken)
    {
        var result = await _createItemUseCase.ExecuteAsync(request, cancellationToken);
        return Ok(result);
    }
}
