using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VehicleTypes.ListVehicleType;

public class ListVehicleTypeHandler(IVehicleTypeRepository repository, ILogger<ListVehicleTypeHandler> logger) : IRequestHandler<ListVehicleTypeQuery, Result<List<VehicleType>>>
{
    private readonly IVehicleTypeRepository _repository = repository;
    private readonly ILogger<ListVehicleTypeHandler> _logger = logger;

    public async Task<Result<List<VehicleType>>> Handle(
        ListVehicleTypeQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleTypeList = await _repository.AllAsync(query.Skip, query.Take);

            _logger.LogInformation("Listagem de tipos de veículo com sucesso.");
            return Result<List<VehicleType>>.Success(vehicleTypeList.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar tipos de veiculo. {@query} {@error}", query, ex.Message);
            return Result<List<VehicleType>>.Failure(ex.Message);
        }
    }
}
