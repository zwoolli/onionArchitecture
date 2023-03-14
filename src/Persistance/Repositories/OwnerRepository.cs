using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;
using System.Data.Common;
namespace Persistance.Repositories;

internal sealed class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
{
    public OwnerRepository(IDBConnector dbConnector) : base(dbConnector) {}

    public Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task InsertAsync(Owner owner)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;
        OwnerTable dto = new OwnerTable(owner);

        string sqlOwner = $@"
                            INSERT INTO {nameof(Owner)} 
                            (
                                {nameof(OwnerTable.name)},
                                {nameof(OwnerTable.date_of_birth)}
                                {nameof(OwnerTable.address)}
                            )
                            VALUES 
                            (
                                @{nameof(OwnerTable.name)},
                                @{nameof(OwnerTable.date_of_birth)},
                                @{nameof(OwnerTable.address)}
                            )";

        string sqlAccount = $@"
                            INSERT INTO {nameof(Account)}
                            (
                                {nameof(AccountTable.owner_id)},
                                {nameof(AccountTable.account_type)},
                                {nameof(AccountTable.date_created)}
                            )
                            VALUES
                            (
                                @{nameof(AccountTable.owner_id)},
                                @{nameof(AccountTable.account_type)},
                                @{nameof(AccountTable.date_created)}
                            )";

        await connection.ExecuteAsync(sqlOwner, dto, transaction);
        await connection.ExecuteAsync(sqlAccount, dto.accounts, transaction);
    }

    public override async Task RemoveAsync(Owner owner)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;
        Guid id = owner.Id;

        string sqlOwner = @$"
                            DELETE
                            FROM {nameof(Owner)}
                            WHERE {nameof(OwnerTable.owner_id)} = @{nameof(id)}";

        string sqlAccount = @$"
                            DELETE
                            FROM {nameof(Account)}
                            WHERE {nameof(AccountTable.owner_id)} = @{nameof(id)}";

        await connection.ExecuteAsync(sqlAccount, new {id}, transaction);
        await connection.ExecuteAsync(sqlOwner, new {id}, transaction);
    }
}