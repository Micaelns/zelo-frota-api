using Domain.Enum;

namespace Domain.Entities;

public class VehicleType(string name, LicenseCategory requeridLicenseCategory = LicenseCategory.None) : Base()
{
    public string Name { get; set; } = name;
    public LicenseCategory RequeridLicenseCategory { get; set; } = requeridLicenseCategory;
}
