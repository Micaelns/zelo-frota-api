using Domain.Enum;

namespace Domain.Entities;

public class VehicleType : Base
{
    public string Name { get; set; }
    public LicenseCategory RequeridLicenseCategory { get; set; }

    public VehicleType(string name, LicenseCategory requeridLicenseCategory = LicenseCategory.None)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new InvalidDataException("O campo Nome não podem ser vazio");
        }
        Name = name;
        RequeridLicenseCategory = requeridLicenseCategory;
    }
}
