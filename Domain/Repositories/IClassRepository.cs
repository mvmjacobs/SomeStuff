using SomeStuff.Domain.Entities;

namespace SomeStuff.Domain.Repositories;

public interface IClassRepository
{
    ClassEntity? FindById(Guid classId);
    bool IncrementEnrollment(Guid classId, int maxCapacity);
    Task<T> ExecuteLockedAsync<T>(Guid classId, Func<T> action);
}
