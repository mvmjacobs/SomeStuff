using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.UseCases.BookClass;

public interface IBookClassUseCase
{
    Task<BookClassResult> ExecuteAsync(string classId, BookingRequestDto request, CancellationToken cancellationToken);
}
