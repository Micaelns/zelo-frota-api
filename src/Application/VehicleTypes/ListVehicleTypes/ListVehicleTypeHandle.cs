using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.VehicleTypes.ListVehicleTypes;

public class ListVehicleTypeHandle(IVehicleTypeRepository repository) : IRequestHandler<ListVehicleTypeQuery, Result<List<VehicleType>>>
{
    private readonly IVehicleTypeRepository _repository = repository;
    public async Task<Result<List<VehicleType>>> Handle(
        ListVehicleTypeQuery command,
        CancellationToken cancellationToken)
    {
        var vehicleTypeList = await _repository.AllAsync(command.Skip, command.Take);

        return Result<List<VehicleType>>.Success(vehicleTypeList.ToList());
    }
}
