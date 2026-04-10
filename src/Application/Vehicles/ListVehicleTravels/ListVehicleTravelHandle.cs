using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.ListVehicleTravels;

public class ListVehicleTravelHandle(ITravelRepository travelRepository, IVehicleRepository vehicleRepository) : IRequestHandler<ListVehicleTravelQuery, Result<List<Travel>>>
{
    private readonly ITravelRepository _travelRepository = travelRepository;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    public async Task<Result<List<Travel>>> Handle(
        ListVehicleTravelQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
                return Result<List<Travel>>.Failure("Veículo não encontrado");

            var vehicleTravelList = await _travelRepository.GetTravelsByVehicleAsync(command.VehicleId, command.Skip, command.Take);

            return Result<List<Travel>>.Success(vehicleTravelList.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<Travel>>.Failure(ex.Message);
        }
    }
}
