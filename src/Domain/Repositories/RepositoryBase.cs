using Domain.Repositories;

namespace Persistance.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected IDbConnector _dbConnector;

    public RepositoryBase(IDbConnector dbConnector)
    {
        this._dbConnector = dbConnector;
    }
    public abstract Task InsertAsync(T item);

    public abstract Task RemoveAsync(T item);
}