using Domain.Repositories;
using System.Data.Common;
using Npgsql;

namespace Persistance.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private string _connectionString;
    private DbTransaction? _transaction;

    public UnitOfWork(IDbConfiguration configuration)
    {
        this._connectionString = configuration.ConnectionString;
        this._transaction = null;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbTransaction t = await this.Transaction();

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

    public async Task<DbTransaction> Transaction()
    {
        if (this._transaction is not null && this._transaction.Connection is not null) return this._transaction;
        if (this._transaction is not null) await this._transaction.DisposeAsync();

        DbConnection connection = new NpgsqlConnection(this._connectionString);
        await connection.OpenAsync();

        this._transaction = await connection.BeginTransactionAsync();
        return this._transaction;
    }
}