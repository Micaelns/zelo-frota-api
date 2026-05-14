namespace Application.DTO.Travel;

public class VehicleEconomyDto
{
    public Guid VehicleId { get; set; }
    public string VehiclePlate { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public double AverageAutonomy { get; set; }
    public int TotalTravels { get; set; }
}
