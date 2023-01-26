namespace Arduino.Common.DomainModel.DomainEventAggregate;

public enum EventStatus
{
    NotProcessed = 0,
    Processed = 1,
    ProcessedWithErrors = 2
}