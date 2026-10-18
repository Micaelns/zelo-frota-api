namespace Application.DTO.Vehicle;

public class VehicleDTO
{
    public Guid Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string RequeridLicenseCategory { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public int QtdTravels { get; set; }
}
