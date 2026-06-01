using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VehicleTypes.ShowVehicleType;

public class ShowVehicleTypeHandler(IVehicleTypeRepository repository, ILogger<ShowVehicleTypeHandler> logger) : IRequestHandler<ShowVehicleTypeQuery, Result<VehicleType>>
{
    private readonly IVehicleTypeRepository _repository = repository;
    private readonly ILogger<ShowVehicleTypeHandler> _logger = logger;

    public async Task<Result<VehicleType>> Handle(
        ShowVehicleTypeQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleType = await _repository.FindAsync(query.Id);
            if (vehicleType is null)
            {
                _logger.LogError("Tipo de veículo {@id} não existe", query.Id);
                return Result<VehicleType>.Failure("Tipo de veículo não existe", ErrorType.Validation);
            }
            _logger.LogInformation("Retornado de tipo de veículo com sucesso.");
            return Result<VehicleType>.Success(vehicleType);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de procurar tipos de veiculo. {@query} {@error}", query, ex.Message);
            return Result<VehicleType>.Failure(ex.Message);
        }
    }
}
