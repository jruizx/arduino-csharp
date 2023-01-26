namespace Arduino.Common.DomainModel;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
        domainEvents = new List<IDomainEvent>();
    }

    public long AutoId { get; private set; }
    public Guid Id { get; protected set; }
    public byte[] RowVersion { get; protected set; }

    private readonly List<IDomainEvent> domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        domainEvents.Add(domainEvent);

    public void ClearEvents() => domainEvents.Clear();

    protected bool Equals(Entity other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}