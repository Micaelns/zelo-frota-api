using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.UseCases.Travels.ListTravel;

public class ListTravelHandle(ITravelQuery travelQuery, IVehicleRepository vehicleRepository) : IRequestHandler<ListTravelQuery, Result<List<Travel>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    public async Task<Result<List<Travel>>> Handle(
        ListTravelQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
                return Result<List<Travel>>.Failure("Veículo não encontrado");

            var vehicleTravelList = await _travelQuery.GetTravelsByVehicleAsync(command.VehicleId,command.Skip, command.Take);

            return Result<List<Travel>>.Success(vehicleTravelList.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<Travel>>.Failure(ex.Message);
        }
    }
}
