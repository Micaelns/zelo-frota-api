using Domain.Enum;

namespace Domain.Entities;

public class VehicleType(string name) : Base()
{
    public string Name { get; set; } = name;
    public LicenseCategory RequeridLicenseCategory { get; set; } = LicenseCategory.None;
}
