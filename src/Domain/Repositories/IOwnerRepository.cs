using Domain.Entities;

namespace Domain.Repositories;

public interface IOwnerRepository : IRepository<Owner>
{
    Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Owner owner);
}
