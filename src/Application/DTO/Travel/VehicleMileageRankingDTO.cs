namespace Application.DTO.Travel;

public class VehicleMileageRankingDTO
{
    public Guid VehicleId { get; set; }
    public string VehiclePlate { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public int TotalMileage { get; set; }
    public int TotalTravels { get; set; }
}
