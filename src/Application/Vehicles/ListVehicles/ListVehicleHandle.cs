using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.ListVehicles;

public class ListVehicleHandle(IVehicleRepository repository) : IRequestHandler<ListVehicleQuery, Result<List<Vehicle>>>
{
    private readonly IVehicleRepository _repository = repository;
    public async Task<Result<List<Vehicle>>> Handle(
        ListVehicleQuery command,
        CancellationToken cancellationToken)
    {
        var vehicleList = await _repository.AllAsync(command.Skip, command.Take);

        return Result<List<Vehicle>>.Success(vehicleList.ToList());
    }
}
