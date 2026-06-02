using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Vehicles.ListVehicle;

public class ListVehicleQuery : IRequest<Result<List<Vehicle>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
