using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;
using System.Data.Common;
namespace Persistance.Repositories;

internal sealed class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(IDBConnector dbConnector) : base(dbConnector) {}

    public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;

        string sql = $@"
                        SELECT  *
                        FROM    {AccountTable.Title}
                        WHERE   {AccountTable.Column.OwnerId} = {nameof(ownerId)}";

        IEnumerable<Account> accounts = await connection.QueryAsync<Account>(sql, new {ownerId}, transaction);

        return accounts;
    }

    public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;

        string sql = $@"
                        SELECT  *
                        FROM    {AccountTable.Title}
                        WHERE   {AccountTable.Title}.{AccountTable.Column.AccountId} = @{nameof(accountId)}";


        Account account = await connection.QuerySingleAsync<Account>(sql, new {accountId}, transaction);

        return account;
    }

    public override async Task InsertAsync(Account account)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;
        AccountTable dto = new AccountTable(account);

        string sql = $@"
                        INSERT INTO {AccountTable.Title} 
                        (
                            {AccountTable.Column.OwnerId},
                            {AccountTable.Column.AccountType}
                            {AccountTable.Column.DateCreated}
                        )
                        VALUES 
                        (
                            @{AccountTable.Column.OwnerId},
                            @{AccountTable.Column.AccountType},
                            @{AccountTable.Column.DateCreated}
                        )";

        await connection.ExecuteAsync(sql, dto, transaction);
    }

    public override async Task RemoveAsync(Account account)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;
        Guid id = account.Id;

        string sql = @$"
                            DELETE
                            FROM {AccountTable.Title}
                            WHERE {AccountTable.Column.AccountId} = @{nameof(id)}";

        await connection.ExecuteAsync(sql, new {id}, transaction);
    }
}