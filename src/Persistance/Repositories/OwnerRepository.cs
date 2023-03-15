using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;
using System.Data.Common;
namespace Persistance.Repositories;

internal sealed class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
{
    public OwnerRepository(IDBConnector dbConnector) : base(dbConnector) {}

    private async Task<IEnumerable<Owner>> _GetAsync(string sql, object? param)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;

        Dictionary<Guid, OwnerTable> ownerDictionary = new Dictionary<Guid, OwnerTable>();

        IEnumerable<OwnerTable> dtos = (
            await connection.QueryAsync<OwnerTable, AccountTable, OwnerTable>(
                sql,
                (ownerDto, accountDto) =>
                {
                    OwnerTable ownerEntry;

                    if (!ownerDictionary.TryGetValue(ownerDto.owner_id, out ownerEntry!))
                    {
                        ownerEntry = ownerDto;
                        ownerEntry.accounts = new List<AccountTable>();
                        ownerDictionary.Add(ownerEntry.owner_id, ownerEntry);
                    }

                    if (accountDto != null) ownerEntry.accounts.Add(accountDto);
                    return ownerEntry;
                },
                transaction: transaction,
                param: param,
                splitOn: AccountTable.Column.AccountId
            )
        )
        .Distinct();

        return dtos.Select(d => d.Adapt());
    }
    public async Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string sql = $@"
                        SELECT      *
                        FROM        {OwnerTable.Title}
                        LEFT JOIN   {AccountTable.Title}
                        ON          ({OwnerTable.Title}.{OwnerTable.Column.OwnerId} = {AccountTable.Title}.{AccountTable.Column.OwnerId})";

        IEnumerable<Owner> owners = await this._GetAsync(sql, null);

        return owners;
    }

    public async Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        string sql = $@"
                        SELECT      *
                        FROM        {OwnerTable.Title}
                        LEFT JOIN   {AccountTable.Title}
                        ON          ({OwnerTable.Title}.{OwnerTable.Column.OwnerId} = {AccountTable.Title}.{AccountTable.Column.OwnerId})
                        WHERE       {OwnerTable.Title}.{OwnerTable.Column.OwnerId} = @{nameof(ownerId)}";

        IEnumerable<Owner> owners = await this._GetAsync(sql, ownerId);

        Owner owner = owners.DefaultIfEmpty(new Owner()).First();

        return owner;
    }

    public override async Task InsertAsync(Owner owner)
    {
        DbTransaction transaction = await this._dbConnector.Transaction();
        DbConnection connection = transaction.Connection!;
        OwnerTable dto = new OwnerTable(owner);

        string sqlOwner = $@"
                            INSERT INTO {OwnerTable.Title} 
                            (
                                {OwnerTable.Column.Name},
                                {OwnerTable.Column.DateOfBirth}
                                {OwnerTable.Column.Address}
                            )
                            VALUES 
                            (
                                @{OwnerTable.Column.Name},
                                @{OwnerTable.Column.DateOfBirth},
                                @{OwnerTable.Column.Address}
                            )";

        string sqlAccount = $@"
                            INSERT INTO {AccountTable.Title}
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
                            FROM {OwnerTable.Title}
                            WHERE {OwnerTable.Column.OwnerId} = @{nameof(id)}";

        string sqlAccount = @$"
                            DELETE
                            FROM {AccountTable.Title}
                            WHERE {AccountTable.Column.OwnerId} = @{nameof(id)}";

        await connection.ExecuteAsync(sqlAccount, new {id}, transaction);
        await connection.ExecuteAsync(sqlOwner, new {id}, transaction);
    }
}