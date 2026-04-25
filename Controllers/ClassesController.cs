using Microsoft.AspNetCore.Mvc;
using SomeStuff.Application.Dtos;
using SomeStuff.Application.UseCases.BookClass;
using SomeStuff.Filters;

namespace SomeStuff.Controllers;

[ApiController]
[Route("api/classes")]
public sealed class ClassesController(IBookClassUseCase bookClassUseCase) : ControllerBase
{
    private readonly IBookClassUseCase _bookClassUseCase = bookClassUseCase;

    [HttpPost("{classId}/book")]
    [ServiceFilter(typeof(BookingRateLimitFilter))]
    public async Task<IActionResult> BookClass(string classId, [FromBody] BookingRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _bookClassUseCase.ExecuteAsync(classId, request, cancellationToken);

        return result.Status switch
        {
            BookClassStatus.Success => StatusCode(StatusCodes.Status201Created, result.Booking),
            BookClassStatus.AlreadyBooked => BadRequest("User already booked this class."),
            BookClassStatus.ClassNotFound => NotFound(),
            BookClassStatus.ClassFull => Conflict("Class is already full."),
            BookClassStatus.InvalidClassId => BadRequest("Invalid classId."),
            BookClassStatus.InvalidUserId => BadRequest("Invalid userId."),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
