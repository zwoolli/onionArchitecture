using Domain.Repositories;

namespace Persistance.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryManager(
        IOwnerRepository ownerRepository, 
        IAccountRepository accountRepository, 
        IUnitOfWork unitOfWork)
    {
        this._ownerRepository = ownerRepository;
        this._accountRepository = accountRepository;
        this._unitOfWork = unitOfWork;
    }

    public IOwnerRepository OwnerRepository => this._ownerRepository;

    public IAccountRepository AccountRepository => this._accountRepository;

    public IUnitOfWork UnitOfWork => this._unitOfWork;
}