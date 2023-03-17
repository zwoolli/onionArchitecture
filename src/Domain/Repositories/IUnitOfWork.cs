using System.Data.Common;
namespace Domain.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<DbTransaction> Transaction();
}