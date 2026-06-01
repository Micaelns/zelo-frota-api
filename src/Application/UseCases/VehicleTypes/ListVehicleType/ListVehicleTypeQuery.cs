using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.VehicleTypes.ListVehicleType;

public class ListVehicleTypeQuery : IRequest<Result<List<VehicleType>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
