using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;
using Mapster;

namespace Services;

public sealed class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository) => this._accountRepository = accountRepository;

    public async Task<AccountDto> CreateAsync(Guid ownerId, AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken = default)
    {
        bool exist = await this._accountRepository.DoesOwnerExist(ownerId, cancellationToken);

        if (!exist)  throw new OwnerNotFoundException(ownerId);

        Account account = accountForCreationDto.Adapt<Account>();

        account.OwnerId = ownerId;

        await this._accountRepository.InsertAsync(account);

        await this._accountRepository.SaveChangesAsync(cancellationToken);

        return account.Adapt<AccountDto>();
    }

    public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
    {
        bool exist = await this._accountRepository.DoesOwnerExist(ownerId, cancellationToken);

        if (!exist)  throw new OwnerNotFoundException(ownerId);

        Account account = await this._accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        if (account.OwnerId != ownerId)
        {
            throw new AccountDoesNotBelongToOwnerException(ownerId, account.Id);
        }

        await this._accountRepository.RemoveAsync(account);

        await this._accountRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AccountDto>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Account> accounts = await this._accountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        IEnumerable<AccountDto> accountDtos = accounts.Adapt<IEnumerable<AccountDto>>();

        return accountDtos;
    }

    public async Task<AccountDto> GetByIdAsync(Guid ownerId, Guid accountID, CancellationToken cancellationToken = default)
    {
        bool exist = await this._accountRepository.DoesOwnerExist(ownerId, cancellationToken);

        if (!exist)  throw new OwnerNotFoundException(ownerId);

        Account account = await this._accountRepository.GetByIdAsync(accountID, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException(accountID);
        }

        if (account.OwnerId != ownerId)
        {
            throw new AccountDoesNotBelongToOwnerException(ownerId, account.Id);
        }

        AccountDto accountDto = account.Adapt<AccountDto>();

        return accountDto;
    }
}