using Domain.Repositories;
using System.Data.Common;
namespace Persistance.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDBConnector _dbConnector;

    public UnitOfWork(IDBConnector dBConnector)
    {
        this._dbConnector = dBConnector;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbTransaction t = await this._dbConnector.Transaction();

        try
        {
            await t.CommitAsync();
        }
        catch
        {
            await t.RollbackAsync();
            throw;
        }
        finally
        {
            await t.DisposeAsync();
        }
    }
}