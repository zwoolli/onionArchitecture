namespace Domain.Repositories;

public interface IRepository<T>
{
    Task InsertAsync(T item);
    Task RemoveAsync(T item);
}