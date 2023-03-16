using Npgsql;
using Domain.Repositories;
using System.Data.Common;

namespace Persistance.Repositories;

public class DbConnector : IDbConnector
{
    private string _connectionString;
    private DbTransaction? _transaction;

    public DbConnector(IDbConfiguration configuration)
    {
        this._connectionString = configuration.ConnectionString;
        this._transaction = null;
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