namespace Application.DTO.Travel;

public class TravelDTO
{
    public Guid IdTravel { get; set; }
    public string VehiclePlate { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public string Vehicle { get; set; } = string.Empty;
    public Guid DestinationId { get; set; }
    public string Destination { get; set; } = string.Empty;
    public int? StartedMileage { get; set; }
    public int? FinishedMileage { get; set; }
    public float? Autonomy { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}
