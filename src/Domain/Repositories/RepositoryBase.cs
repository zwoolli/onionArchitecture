using Domain.Repositories;

namespace Persistance.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    private IDBConnector _dbConnector;

    public RepositoryBase(IDBConnector dbConnector)
    {
        this._dbConnector = dbConnector;
    }
    public abstract Task InsertAsync(T item);

    public abstract Task RemoveAsync(T item);
}