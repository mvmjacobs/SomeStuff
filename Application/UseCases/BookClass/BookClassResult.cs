using SomeStuff.Application.Dtos;

namespace SomeStuff.Application.UseCases.BookClass;

public sealed record BookClassResult(BookClassStatus Status, BookingResponseDto? Booking = null);
