using System.Collections.Concurrent;
using SomeStuff.Domain.Entities;
using SomeStuff.Domain.Repositories;

namespace SomeStuff.Infrastructure.Repositories;

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<string, BookingEntity> _bookings = new();

    public BookingEntity? FindByClassAndUser(Guid classId, Guid userId)
        => _bookings.TryGetValue(BuildKey(classId, userId), out var booking) ? booking : null;

    public void Add(BookingEntity booking)
        => _bookings[BuildKey(booking.ClassId, booking.UserId)] = booking;

    private static string BuildKey(Guid classId, Guid userId) => $"{classId:N}:{userId:N}";
}
