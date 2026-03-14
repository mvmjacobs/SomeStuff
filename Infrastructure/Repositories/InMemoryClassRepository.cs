using System.Collections.Concurrent;
using SomeStuff.Domain.Entities;
using SomeStuff.Domain.Repositories;

namespace SomeStuff.Infrastructure.Repositories;

public sealed class InMemoryClassRepository : IClassRepository
{
    private readonly ConcurrentDictionary<Guid, ClassEntity> _classes = new();
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

    public ClassEntity? FindById(Guid classId)
        => _classes.TryGetValue(classId, out var classEntity) ? classEntity : null;

    public bool IncrementEnrollment(Guid classId, int maxCapacity)
    {
        var classEntity = FindById(classId);
        if (classEntity is null || classEntity.EnrolledCount >= maxCapacity)
        {
            return false;
        }

        classEntity.IncrementEnrollment();
        return true;
    }

    public async Task<T> ExecuteLockedAsync<T>(Guid classId, Func<T> action)
    {
        var semaphore = _locks.GetOrAdd(classId, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();

        try
        {
            return action();
        }
        finally
        {
            semaphore.Release();
        }
    }

    public void Seed(ClassEntity classEntity)
        => _classes[classEntity.Id] = classEntity;
}
