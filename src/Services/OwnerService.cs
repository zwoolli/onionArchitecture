using Services.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Contracts;
using Mapster;

namespace Services;

internal sealed class OwnerService : IOwnerService
{
    private readonly IRepositoryManager _repositoryManager;

    public OwnerService(IRepositoryManager repositoryManager)
    {
        this._repositoryManager = repositoryManager;
    }

    public async Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default)
    {
        Owner owner = ownerForCreationDto.Adapt<Owner>();

        this._repositoryManager.OwnerRepository.Insert(owner);

        await this._repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return owner.Adapt<OwnerDto>();
    }

    public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        this._repositoryManager.OwnerRepository.Remove(owner);

        await this._repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Owner> owners = await this._repositoryManager.OwnerRepository.GetAllAsync(cancellationToken);

        IEnumerable<OwnerDto> ownersDto = owners.Adapt<IEnumerable<OwnerDto>>();

        return ownersDto;
    }

    public async Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        OwnerDto ownerDto = owner.Adapt<OwnerDto>();

        return ownerDto;
    }

    public async Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        owner.Name = ownerForUpdateDto.Name;
        owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
        owner.Address = ownerForUpdateDto.Address;

        await this._repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}