using Application.Contracts.Abstractions.Vehicles.Query;
using Application.DTO;
using Application.DTO.Vehicle;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.ListVehicle;

public class ListVehicleHandler(IVehicleQuery vehicleQuery, ILogger<ListVehicleHandler> logger) : IRequestHandler<ListVehicleQuery, Result<List<VehicleDTO>>>
{
    private readonly IVehicleQuery _vehicleQuery = vehicleQuery;
    private readonly ILogger<ListVehicleHandler> _logger = logger;

    public async Task<Result<List<VehicleDTO>>> Handle(
        ListVehicleQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var pagination = new Pagination(query.Page, query.Take);
            var (vehicleList, totalItems) = await _vehicleQuery.AllAsync(pagination.CurrentPage, pagination.PerPage);
            pagination.TotalItems = totalItems;

            if (vehicleList.Count() == 0)
            {
                _logger.LogInformation("Listagem de viagens não contem dados.");
                return Result<List<VehicleDTO>>.Failure("Página não contem dados",ErrorType.NotFound);
            }
            _logger.LogInformation("Listagem de viagens foi finalizado com sucesso.");
            return Result<List<VehicleDTO>>.Success(vehicleList.ToList(), pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar viagens. {@error}", ex.Message);
            return Result<List<VehicleDTO>>.Failure(ex.Message, ErrorType.Internal);
        }
    }
}
