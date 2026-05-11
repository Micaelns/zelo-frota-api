namespace Application.Contracts.Abstractions;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}
