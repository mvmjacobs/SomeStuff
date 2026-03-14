using SomeStuff.Application.Dtos;
using SomeStuff.Domain.Entities;
using SomeStuff.Domain.Repositories;

namespace SomeStuff.Application.UseCases.BookClass;

public sealed class BookClassUseCase(IClassRepository classRepository, IBookingRepository bookingRepository) : IBookClassUseCase
{
    private readonly IClassRepository _classRepository = classRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;

    public Task<BookClassResult> ExecuteAsync(string classId, BookingRequestDto request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(classId, out var classGuid))
        {
            return Task.FromResult(new BookClassResult(BookClassStatus.InvalidClassId));
        }

        if (request is null || string.IsNullOrWhiteSpace(request.UserId) || !Guid.TryParse(request.UserId, out var userGuid))
        {
            return Task.FromResult(new BookClassResult(BookClassStatus.InvalidUserId));
        }

        return _classRepository.ExecuteLockedAsync(classGuid, () =>
        {
            var classEntity = _classRepository.FindById(classGuid);
            if (classEntity is null)
            {
                return new BookClassResult(BookClassStatus.ClassNotFound);
            }

            var existingBooking = _bookingRepository.FindByClassAndUser(classGuid, userGuid);
            if (existingBooking is not null)
            {
                return new BookClassResult(
                    BookClassStatus.AlreadyBooked,
                    MapBooking(existingBooking, wasCreated: false));
            }

            if (!classEntity.HasCapacity())
            {
                return new BookClassResult(BookClassStatus.ClassFull);
            }

            var booking = new BookingEntity
            {
                Id = Guid.NewGuid(),
                ClassId = classGuid,
                UserId = userGuid,
                Timestamp = DateTime.UtcNow
            };

            _bookingRepository.Add(booking);
            _classRepository.IncrementEnrollment(classGuid, classEntity.Capacity);

            return new BookClassResult(
                BookClassStatus.Success,
                MapBooking(booking, wasCreated: true));
        });
    }

    private static BookingResponseDto MapBooking(BookingEntity booking, bool wasCreated)
        => new(booking.Id, booking.ClassId, booking.UserId, booking.Timestamp, wasCreated);
}
