using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.UseCases.VehicleTypes.ListVehicleType;

public class ListVehicleTypeHandle(IVehicleTypeRepository repository) : IRequestHandler<ListVehicleTypeQuery, Result<List<VehicleType>>>
{
    private readonly IVehicleTypeRepository _repository = repository;
    public async Task<Result<List<VehicleType>>> Handle(
        ListVehicleTypeQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleTypeList = await _repository.AllAsync(query.Skip, query.Take);

            return Result<List<VehicleType>>.Success(vehicleTypeList.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<VehicleType>>.Failure(ex.Message);
        }
    }
}
