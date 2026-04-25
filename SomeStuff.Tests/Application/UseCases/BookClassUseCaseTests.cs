using SomeStuff.Application.Dtos;
using SomeStuff.Application.UseCases.BookClass;
using SomeStuff.Domain.Entities;
using SomeStuff.Infrastructure.Repositories;

namespace SomeStuff.Tests.Application.UseCases;

public sealed class BookClassUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_CreatesBooking_WhenClassHasCapacity()
    {
        var classRepository = new InMemoryClassRepository();
        var bookingRepository = new InMemoryBookingRepository();
        var useCase = new BookClassUseCase(classRepository, bookingRepository);
        var classId = Guid.NewGuid();

        classRepository.Seed(new ClassEntity
        {
            Id = classId,
            Title = "Pilates",
            Capacity = 20
        });

        var result = await useCase.ExecuteAsync(classId.ToString(), new BookingRequestDto(Guid.NewGuid().ToString()), CancellationToken.None);

        Assert.Equal(BookClassStatus.Success, result.Status);
        Assert.NotNull(result.Booking);
        Assert.Equal(1, classRepository.FindById(classId)!.EnrolledCount);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsExistingBooking_WhenUserAlreadyBooked()
    {
        var classRepository = new InMemoryClassRepository();
        var bookingRepository = new InMemoryBookingRepository();
        var useCase = new BookClassUseCase(classRepository, bookingRepository);
        var classId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        classRepository.Seed(new ClassEntity
        {
            Id = classId,
            Title = "Yoga",
            Capacity = 20
        });

        var firstResult = await useCase.ExecuteAsync(classId.ToString(), new BookingRequestDto(userId), CancellationToken.None);
        var secondResult = await useCase.ExecuteAsync(classId.ToString(), new BookingRequestDto(userId), CancellationToken.None);

        Assert.Equal(BookClassStatus.Success, firstResult.Status);
        Assert.Equal(BookClassStatus.AlreadyBooked, secondResult.Status);
        Assert.Null(secondResult.Booking);
        Assert.Equal(1, classRepository.FindById(classId)!.EnrolledCount);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsNotFound_WhenClassDoesNotExist()
    {
        var useCase = new BookClassUseCase(new InMemoryClassRepository(), new InMemoryBookingRepository());

        var result = await useCase.ExecuteAsync(Guid.NewGuid().ToString(), new BookingRequestDto(Guid.NewGuid().ToString()), CancellationToken.None);

        Assert.Equal(BookClassStatus.ClassNotFound, result.Status);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsConflict_WhenClassIsFull()
    {
        var classRepository = new InMemoryClassRepository();
        var bookingRepository = new InMemoryBookingRepository();
        var useCase = new BookClassUseCase(classRepository, bookingRepository);
        var classId = Guid.NewGuid();

        classRepository.Seed(new ClassEntity
        {
            Id = classId,
            Title = "Spin",
            Capacity = 1
        });

        await useCase.ExecuteAsync(classId.ToString(), new BookingRequestDto(Guid.NewGuid().ToString()), CancellationToken.None);
        var result = await useCase.ExecuteAsync(classId.ToString(), new BookingRequestDto(Guid.NewGuid().ToString()), CancellationToken.None);

        Assert.Equal(BookClassStatus.ClassFull, result.Status);
        Assert.Equal(1, classRepository.FindById(classId)!.EnrolledCount);
    }

    [Fact]
    public async Task ExecuteAsync_AllowsOnlyOneSuccess_WhenRequestsRaceForFinalSpot()
    {
        var classRepository = new InMemoryClassRepository();
        var bookingRepository = new InMemoryBookingRepository();
        var useCase = new BookClassUseCase(classRepository, bookingRepository);
        var classId = Guid.NewGuid();

        classRepository.Seed(new ClassEntity
        {
            Id = classId,
            Title = "Crossfit",
            Capacity = 1
        });

        var tasks = Enumerable.Range(0, 50)
            .Select(_ => useCase.ExecuteAsync(
                classId.ToString(),
                new BookingRequestDto(Guid.NewGuid().ToString()),
                CancellationToken.None))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        Assert.Equal(1, results.Count(r => r.Status == BookClassStatus.Success));
        Assert.Equal(49, results.Count(r => r.Status == BookClassStatus.ClassFull));
        Assert.Equal(1, classRepository.FindById(classId)!.EnrolledCount);
    }
}
