using Domain.Entities;

namespace Api.Requests.Vehicles;
public class StartTravelRequest
{
    public Guid DestinationId { get; set; }
    public Guid? DriverId { get; set; }
    public DateTime? WhenTravel { get; set; }
}
