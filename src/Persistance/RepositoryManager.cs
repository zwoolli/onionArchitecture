using Domain.Repositories;

namespace Persistance.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _connectionString;

//TODO figure out how repositories and unit of work can share a transaction
//https://stackoverflow.com/questions/38440974/should-i-create-a-brand-new-sqlconnection-each-time-i-want-to-use-it-or-just-at
    public RepositoryManager(
        IDBconfiguration configuration,
        IOwnerRepository ownerRepository, 
        IAccountRepository accountRepository, 
        IUnitOfWork unitOfWork)
    {
        this._connectionString = configuration.ConnectionString;
        this._ownerRepository = ownerRepository;
        this._accountRepository = accountRepository;
        this._unitOfWork = unitOfWork;
    }

    public IOwnerRepository OwnerRepository => throw new NotImplementedException();

    public IAccountRepository AccountRepository => throw new NotImplementedException();

    public IUnitOfWork UnitOfWork => throw new NotImplementedException();
}