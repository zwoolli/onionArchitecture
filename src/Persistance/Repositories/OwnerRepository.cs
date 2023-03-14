using Dapper;
using Domain.Repositories;
using Domain.Entities;
using Persistance.Tables;

namespace Persistance.Repositories;

// Add schema folder back to make queries easier
//https://github.com/zwoolli/beamBuddy/blob/main/src/DataAccess/SectionRepository.cs
internal sealed class OwnerRepository : Repository<Owner>, IOwnerRepository
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

    public override Task InsertAsync(Owner owner)
    {
        OwnerTable dto = new OwnerTable(owner);

        string sqlOwner = $@"
                            INSERT INTO {nameof(Owner)} (
                                {nameof(OwnerTable.owner_id)},
                                {nameof(OwnerTable.name)},
                                {nameof(OwnerTable.date_of_birth)}
                                {nameof(OwnerTable.address)}
                            )";

//TODO WRITE THIS
        string sqlAccount = $@"
                            ";
        throw new NotImplementedException();
    }

    public override Task RemoveAsync(Owner item)
    {
        throw new NotImplementedException();
    }
}