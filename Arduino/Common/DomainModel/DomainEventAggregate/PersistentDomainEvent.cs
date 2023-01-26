using Newtonsoft.Json;

namespace Arduino.Common.DomainModel.DomainEventAggregate;

public class PersistentDomainEvent : Entity
{
    protected PersistentDomainEvent()
    {
        Errors = new List<string>();
    }

    public PersistentDomainEvent(IDomainEvent domainEvent) : this()
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        TypeName = domainEvent.GetType().AssemblyQualifiedName;

        Data = JsonConvert.SerializeObject(domainEvent);
        Status = EventStatus.NotProcessed;
        Created = domainEvent.Created;
    }

    public string TypeName { get; private set; }
    public string Data { get; private set; }
    public EventStatus Status { get; private set; }
    public int Attempts { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime? LastExecution { get; private set; }
    public List<string> Errors { get; private set; }

    public IDomainEvent GetDomainEvent()
    {
        var domainEventType = Type.GetType(TypeName);
        if (domainEventType == null) throw new InvalidDomainEventException($"Type {TypeName} could not be found");

        return (IDomainEvent)JsonConvert.DeserializeObject(Data, domainEventType);
    }

    public void MarkAsProcessed()
    {
        Status = EventStatus.Processed;
        LastExecution = DateTime.UtcNow;
    }

    public bool IsProcessed() => Status == EventStatus.Processed;

    public void MarkAsFailed()
    {
        Status = EventStatus.ProcessedWithErrors;
        LastExecution = DateTime.UtcNow;
    }

    public bool IsFailed() => Status == EventStatus.ProcessedWithErrors;

    public void RegisterFailure(string error)
    {
        Attempts++;
        Errors.Add(error);
    }

}