using Application.Contracts.Messaging;
using Application.Helpers;

namespace Application.Contracts.Events;

public class TravelStartedEvent: IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public Guid TravelId { get; init; }
    public Guid VehicleId { get; init; }
    public Guid DestinationId { get; init; }
    public int? StartedMileage { get; init; }
    public DateTime? Start { get; init; }
}
