namespace Application.Contracts.Messaging;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}
