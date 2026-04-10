using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.EndsVehiclesTravel;

public class EndsVehicleTravelHandler(ITravelRepository repository) : IRequestHandler<EndsVehicleTravelCommand, Result<Travel>>
{
    private readonly ITravelRepository _repository = repository;

    public async Task<Result<Travel>> Handle(
        EndsVehicleTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var travel = await _repository.FindAsync(command.TravelId);

            if (travel is null)
                return Result<Travel>.Failure("Viagem não encontrada");

            travel.Ends(command.FinishMileage,command.FuelQTD, command.whenArrived);

            await _repository.UpdateAsync(travel);

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }
}
