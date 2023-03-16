using Domain.Repositories;
using System.Data.Common;
namespace Persistance.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnector _dbConnector;

    public UnitOfWork(IDbConnector dBConnector)
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