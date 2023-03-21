using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;
using System.Data.Common;
namespace Persistance.Repositories;

public sealed class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
{
    public OwnerRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}

    private async Task<IEnumerable<Owner>> _GetAsync(string sql, object? param)
    {
        DbTransaction transaction = await this.Transaction();
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

                    if (accountDto != null) ownerEntry.accounts?.Add(accountDto);
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
                        FROM        {OwnerTable.Name}
                        LEFT JOIN   {AccountTable.Name}
                        ON          ({OwnerTable.Name}.{OwnerTable.Column.OwnerId} = {AccountTable.Name}.{AccountTable.Column.OwnerId})";

        IEnumerable<Owner> owners = await this._GetAsync(sql, null);

        return owners;
    }

    public async Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        string sql = $@"
                        SELECT      *
                        FROM        {OwnerTable.Name}
                        LEFT JOIN   {AccountTable.Name}
                        ON          ({OwnerTable.Name}.{OwnerTable.Column.OwnerId} = {AccountTable.Name}.{AccountTable.Column.OwnerId})
                        WHERE       {OwnerTable.Name}.{OwnerTable.Column.OwnerId} = @{nameof(ownerId)}";

        IEnumerable<Owner> owners = await this._GetAsync(sql, new { ownerId });

        Owner owner = owners.DefaultIfEmpty(new Owner()).First();

        return owner;
    }

    public override async Task InsertAsync(Owner owner)
    {
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;
        OwnerTable dto = new OwnerTable(owner);

        string sql = $@"
                            INSERT INTO {OwnerTable.Name} 
                            (
                                {OwnerTable.Column.Name},
                                {OwnerTable.Column.DateOfBirth},
                                {OwnerTable.Column.Address}
                            )
                            VALUES 
                            (
                                @{OwnerTable.Column.Name},
                                @{OwnerTable.Column.DateOfBirth},
                                @{OwnerTable.Column.Address}
                            )";

        await connection.ExecuteAsync(sql, dto, transaction);
    }

    public override async Task RemoveAsync(Owner owner)
    {
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;
        Guid id = owner.Id;

        string sqlOwner = @$"
                            DELETE
                            FROM {OwnerTable.Name}
                            WHERE {OwnerTable.Column.OwnerId} = @{nameof(id)}";

        string sqlAccount = @$"
                            DELETE
                            FROM {AccountTable.Name}
                            WHERE {AccountTable.Column.OwnerId} = @{nameof(id)}";

        await connection.ExecuteAsync(sqlAccount, new {id}, transaction);
        await connection.ExecuteAsync(sqlOwner, new {id}, transaction);
    }

    public async Task UpdateAsync(Owner owner)
    {
        DbTransaction transaction = await this.Transaction();
        DbConnection connection = transaction.Connection!;
        OwnerTable dto = new OwnerTable(owner);

        string sql = @$"
                        UPDATE  {OwnerTable.Name}
                        SET     
                            {OwnerTable.Column.Name} = @{nameof(dto.name)},
                            {OwnerTable.Column.DateOfBirth} = @{nameof(dto.date_of_birth)},
                            {OwnerTable.Column.Address} = @{nameof(dto.address)}
                        WHERE {OwnerTable.Column.OwnerId} = @{nameof(dto.owner_id)}";

        await connection.ExecuteAsync(sql, dto, transaction);
    }
}