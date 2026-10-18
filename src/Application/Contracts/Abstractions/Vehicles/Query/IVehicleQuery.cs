using Application.DTO.Vehicle;

namespace Application.Contracts.Abstractions.Vehicles.Query;

public interface IVehicleQuery
{
    public Task<(IEnumerable<VehicleDTO>, int totalItems)> AllAsync(int page, int take);
}
