namespace Domain.Interfaces.Repository;

public interface IBaseRepository<T>
{
    public Task<IEnumerable<T>> All(int skip, int take = 10);
    public Task Delete(Guid id);
    public Task<T?> Find(Guid id);
    public Task Save(T value);
}
