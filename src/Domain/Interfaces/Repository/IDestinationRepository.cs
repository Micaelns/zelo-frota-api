using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface IDestinationRepository : IBaseRepository<Destination>
{
    public Task<Destination?> GetByZipCodeAsync(string zipCode);
}
