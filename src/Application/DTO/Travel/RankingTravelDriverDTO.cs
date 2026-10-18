namespace Application.DTO.Travel;

public class RankingTravelDriverDTO
{
    public Guid VehicleId { get; set; }
    public string VehicleType { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public int TotalMileage { get; set; }
    public int TotalTravels { get; set; }
    public DateTime? DateFirstTravel { get; set; }
    public DateTime? DateLastTravel { get; set; }
}
