using Application.DTO;
using MediatR;

namespace Application.Vehicles.CreateVehicles;

public class CreateVehicleCommand : IRequest<Result<Guid>>
{
    public string Plate { get; set; } = string.Empty;
    public int InitialMileage { get; set; }
    public required Guid Type { get; set; }
}