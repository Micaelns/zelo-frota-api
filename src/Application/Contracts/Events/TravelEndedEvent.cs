using Application.Contracts.Messaging;

namespace Application.Contracts.Events;

public class TravelEndedEvent: IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public Guid TravelId { get; init; }
    public Guid VehicleId { get; init; }
    public Guid DestinationId { get; init; }
    public int? StartedMileage { get; init; }
    public int? FinishedMileage { get; init; }
    public float? Autonomy { get; init; }
    public DateTime? Start { get; init; }
    public DateTime? End { get; init; }
}
