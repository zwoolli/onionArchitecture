using Npgsql;
using Domain.Repositories;
using System.Data.Common;

namespace Persistance.Repositories;

public class DBConnector : IDBConnector
{
    private string _connectionString;
    private DbConnection? _connection;
    private DbTransaction? _transaction;

    public DBConnector(IDBconfiguration configuration)
    {
        this._connectionString = configuration.ConnectionString;
    }

    public async Task<DbTransaction> Transaction()
    {
        if (this._transaction is not null) return this._transaction;

        this._connection = new NpgsqlConnection(this._connectionString);
        await this._connection.OpenAsync();
        this._transaction = await this._connection.BeginTransactionAsync();
        return this._transaction;
    }


}