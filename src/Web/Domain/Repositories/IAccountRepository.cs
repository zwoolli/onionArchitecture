using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using Domain.Entities;

namespace Domain.Repositories;
public interface IAccountRepository 
{
    public Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    public Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    void Insert(Account account);
    void Remove(Account account);
}