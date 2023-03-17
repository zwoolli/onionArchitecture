using Domain.Entities;

namespace Domain.Repositories;
public interface IAccountRepository : IRepository<Account>
{
    public Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    public Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    public Task<bool> DoesOwnerExist(Guid ownerId, CancellationToken cancellationToken = default);
}