using Application.DTO;
using Application.DTO.Vehicle;
using MediatR;

namespace Application.UseCases.Vehicles.ListVehicle;

public class ListVehicleQuery : IRequest<Result<List<VehicleDTO>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
