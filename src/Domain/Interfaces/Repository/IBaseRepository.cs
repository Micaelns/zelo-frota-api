namespace Domain.Interfaces.Repository;

public interface IBaseRepository<T>
{
    public Task<(IEnumerable<T>, int totalItems)> AllAsync(int page, int take);
    public Task<T?> FindAsync(Guid id);
    public Task AddAsync(T value);
    public Task UpdateAsync(T value);
    public Task DeleteLogicalAsync(Guid id);
}
