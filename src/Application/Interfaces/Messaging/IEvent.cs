namespace Application.Interfaces.Messaging;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}
