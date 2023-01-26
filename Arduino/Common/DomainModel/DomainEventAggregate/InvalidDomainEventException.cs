namespace Arduino.Common.DomainModel.DomainEventAggregate;

public class InvalidDomainEventException : Exception
{
    public InvalidDomainEventException()
    {
    }

    public InvalidDomainEventException(string message)
        : base(message)
    {
    }

    public InvalidDomainEventException(string message, Exception inner)
        : base(message, inner)
    {
    }
}