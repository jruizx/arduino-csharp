namespace Arduino.Common.DomainModel;

public abstract class DomainEvent : ValueObject, IDomainEvent
{
    protected DomainEvent()
    {
        Created = DateTime.UtcNow;
    }
    public DateTime Created { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Created;
    }
}