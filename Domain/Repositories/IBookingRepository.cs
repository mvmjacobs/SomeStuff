using SomeStuff.Domain.Entities;

namespace SomeStuff.Domain.Repositories;

public interface IBookingRepository
{
    BookingEntity? FindByClassAndUser(Guid classId, Guid userId);
    void Add(BookingEntity booking);
}
