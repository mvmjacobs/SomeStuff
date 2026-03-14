namespace SomeStuff.Domain.Entities;

public sealed class BookingEntity
{
    public Guid Id { get; init; }
    public Guid ClassId { get; init; }
    public Guid UserId { get; init; }
    public DateTime Timestamp { get; init; }
}
