using Domain.Enum;

namespace Api.Requests.VehicleTypes;

public class VehicleTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public LicenseCategory RequeridLicenseCategory { get; set; } = LicenseCategory.None;
}
