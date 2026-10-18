using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Drivers.ListDriver;

public class ListDriverHandler(IDriverRepository driverRepository, ILogger<ListDriverHandler> logger) : IRequestHandler<ListDriverQuery, Result<List<Driver>>>
{
    private readonly IDriverRepository _driverRepository = driverRepository;
    private readonly ILogger<ListDriverHandler> _logger = logger;
    public async Task<Result<List<Driver>>> Handle(
        ListDriverQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var pagination = new Pagination(query.Page, query.Take);
            var (driverList, totalItems) = await _driverRepository.AllAsync(pagination.CurrentPage, pagination.PerPage);
            pagination.TotalItems = totalItems;

            if (driverList.Count() == 0)
            {
                _logger.LogInformation("Listagem de motorista não contem dados.");
                return Result<List<Driver>>.Failure("Página não contem dados", ErrorType.NotFound);
            }

            _logger.LogInformation("Sucesso na listagem dos motoristas. {@query}", query);
            return Result<List<Driver>>.Success(driverList.ToList(), pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar motoristas. {@error}", ex.Message);
            return Result<List<Driver>>.Failure(ex.Message);
        }
    }
}
