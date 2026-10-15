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
        ListDriverQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var driverList = await _driverRepository.AllAsync(command.Page, command.Take);
            var totalItems = await _driverRepository.AllContAsync();
            var pagination = new Pagination(totalItems, command.Page, command.Take);

            _logger.LogInformation("Lista de motoristas foi finalizada com sucesso.");
            return Result<List<Driver>>.Success(driverList.ToList(), pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar motoristas. {@error}", ex.Message);
            return Result<List<Driver>>.Failure(ex.Message);
        }
    }
}
