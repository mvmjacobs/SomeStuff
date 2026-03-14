namespace SomeStuff.Domain.Entities;

public sealed class ClassEntity
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Capacity { get; init; } = 20;
    public int EnrolledCount { get; private set; }

    public bool HasCapacity() => EnrolledCount < Capacity;

    public void IncrementEnrollment()
    {
        if (!HasCapacity())
        {
            throw new InvalidOperationException("Class capacity has been reached.");
        }

        EnrolledCount++;
    }
}
