namespace Arduino.Common.DomainModel;

public interface IDomainEvent
{
    DateTime Created { get; }
}