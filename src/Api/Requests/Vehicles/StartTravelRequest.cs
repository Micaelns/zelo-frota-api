namespace Api.Requests.Vehicles;
public class StartTravelRequest
{
    public Guid DestinationId { get; set; }
    public DateTime? WhenTravel { get; set; }
}
