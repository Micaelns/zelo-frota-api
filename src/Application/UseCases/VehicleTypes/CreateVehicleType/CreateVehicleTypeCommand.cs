using Application.DTO;
using Domain.Enum;
using MediatR;

namespace Application.UseCases.VehicleTypes.CreateVehicleType;

public class CreateVehicleTypeCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public LicenseCategory RequeridLicenseCategory { get; set; } = LicenseCategory.None;
}
