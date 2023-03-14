using Services.Abstractions;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly IOwnerService _ownerService;
    private readonly IAccountService _accountService;

    public ServiceManager(IOwnerService ownerService, IAccountService accountService)
    {
        this._ownerService = ownerService;
        this._accountService = accountService;
    }
    public IOwnerService OwnerService => this._ownerService;

    public IAccountService AccountService => this._accountService;
}