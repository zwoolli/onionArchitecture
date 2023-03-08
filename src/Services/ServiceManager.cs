using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IOwnerService> _lazyOwnerService;
    private readonly Lazy<IAccountService> _lazyAccountService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        this._lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));
        this._lazyAccountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
    }
    public IOwnerService OwnerService => this._lazyOwnerService.Value;

    public IAccountService AccountService => this._lazyAccountService.Value;
}