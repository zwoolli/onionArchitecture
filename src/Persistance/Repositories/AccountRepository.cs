using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;
using System.Data.Common;
namespace Persistance.Repositories;

public sealed class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}

    public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;

        string sql = $@"
                        SELECT  *
                        FROM    {AccountTable.Name}
                        WHERE   {AccountTable.Column.OwnerId} = @{nameof(ownerId)}";

        IEnumerable<AccountTable> accounts = await connection.QueryAsync<AccountTable>(sql, new {ownerId}, transaction);

        return accounts.Select(a => a.Adapt());
    }

    public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;

        string sql = $@"
                        SELECT  *
                        FROM    {AccountTable.Name}
                        WHERE   {AccountTable.Name}.{AccountTable.Column.AccountId} = @{nameof(accountId)}";


        AccountTable account = await connection.QuerySingleAsync<AccountTable>(sql, new {accountId}, transaction);

        return account.Adapt();
    }

    public override async Task InsertAsync(Account account)
    {
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;
        AccountTable dto = new AccountTable(account);

        string sql = $@"
                        INSERT INTO {AccountTable.Name} 
                        (
                            {AccountTable.Column.OwnerId},
                            {AccountTable.Column.AccountType},
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
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;
        Guid id = account.Id;

        string sql = @$"
                            DELETE
                            FROM {AccountTable.Name}
                            WHERE {AccountTable.Column.AccountId} = @{nameof(id)}";

        await connection.ExecuteAsync(sql, new {id}, transaction);
    }

    public async Task<bool> DoesOwnerExist(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;

        string sql = $@"
                        SELECT EXISTS(
                            SELECT  *
                            FROM    {OwnerTable.Name}
                            WHERE   {OwnerTable.Column.OwnerId} = @{nameof(ownerId)}
                        )";

        return await connection.ExecuteScalarAsync<bool>(sql, new { ownerId }, transaction);
    }
}