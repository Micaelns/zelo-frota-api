using Application.Contracts.Messaging;

namespace Application.Contracts.Events;

public class TravelReportEvent: IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public Guid? VehicleId { get; set; }
    public Guid? DestinationId { get; set; }
    public DateTime? MonthYearTravel { get; set; }
}
