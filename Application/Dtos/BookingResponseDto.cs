namespace SomeStuff.Application.Dtos;

public sealed record BookingResponseDto(
    Guid BookingId,
    Guid ClassId,
    Guid UserId,
    DateTime Timestamp,
    bool WasCreated);
