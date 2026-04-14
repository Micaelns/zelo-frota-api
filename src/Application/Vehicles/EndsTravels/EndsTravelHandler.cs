using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.EndsTravels;

public class EndsTravelHandler(IVehicleRepository repository, ITravelQuery travelQuery) : IRequestHandler<EndsTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly ITravelQuery _travelQuery = travelQuery;


    public async Task<Result<Travel>> Handle(
        EndsTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _repository.FindAsync(command.VehicleId);

            if (vehicle is null)
                return Result<Travel>.Failure("Veículo não encontrado");

            var travel = await _travelQuery.GetOpenTravelInVehicleAsync(command.VehicleId);

            if (travel is null)
                return Result<Travel>.Failure("Não existe viagem em andamento");

            vehicle.EndTravel(travel, command.FinishMileage, command.FuelQTD, command.WhenArrived);

            await _repository.SaveChangesAsync();

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }
}
