namespace Domain.Interfaces.Repository;

public interface IBaseRepository<T>
{
    public Task<IEnumerable<T>> AllAsync(int skip, int take = 10);
    public Task<T?> FindAsync(Guid id);
    public Task AddAsync(T value);
    public Task UpdateAsync(T value);
    //public Task DeleteAsync(Guid id);
}
