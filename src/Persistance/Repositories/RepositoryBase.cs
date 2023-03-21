using Domain.Repositories;
using System.Data.Common;
namespace Persistance.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    private IUnitOfWork _unitOfWork;

    public RepositoryBase(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
    public abstract Task InsertAsync(T item);

    public abstract Task RemoveAsync(T item);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
    }

    protected async Task<DbTransaction> Transaction()
    {
        return await this._unitOfWork.GetDbTransaction();
    }
}