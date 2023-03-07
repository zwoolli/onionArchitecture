using System.Threading;
using System.Threading.Tasks;

namespace doma.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}