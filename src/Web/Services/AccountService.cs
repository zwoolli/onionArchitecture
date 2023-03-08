using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;
using Mapster;

namespace Services;

internal sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager _repositoryManager;

    public AccountService(IRepositoryManager repositoryManager) => this._repositoryManager = repositoryManager;

    public async Task<AccountDto> CreateAsync(Guid ownerId, AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        Account account = accountForCreationDto.Adapt<Account>();

        account.OwnerId = owner.Id;

        this._repositoryManager.AccountRepository.Insert(account);

        await this._repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return account.Adapt<AccountDto>();
    }

    public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        Account account = await this._repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        if (account.OwnerId != owner.Id)
        {
            throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
        }

        this._repositoryManager.AccountRepository.Remove(account);

        await this._repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AccountDto>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Account> accounts = await this._repositoryManager.AccountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        IEnumerable<AccountDto> accountDtos = accounts.Adapt<IEnumerable<AccountDto>>();

        return accountDtos;
    }

    public async Task<AccountDto> GetByIdAsync(Guid ownerId, Guid accountID, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        Account account = await this._repositoryManager.AccountRepository.GetByIdAsync(accountID, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException(accountID);
        }

        if (account.OwnerId != owner.Id)
        {
            throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
        }

        AccountDto accountDto = account.Adapt<AccountDto>();

        return accountDto;
    }
}