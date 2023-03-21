using Services.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Contracts;
using Mapster;

namespace Services;

public sealed class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        this._ownerRepository = ownerRepository;
    }

    public async Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default)
    {
        Owner owner = ownerForCreationDto.Adapt<Owner>();

        await this._ownerRepository.InsertAsync(owner);

        await this._ownerRepository.SaveChangesAsync(cancellationToken);

        return owner.Adapt<OwnerDto>();
    }

    public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._ownerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        await this._ownerRepository.RemoveAsync(owner);

        await this._ownerRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Owner> owners = await this._ownerRepository.GetAllAsync(cancellationToken);

        IEnumerable<OwnerDto> ownersDto = owners.Adapt<IEnumerable<OwnerDto>>();

        return ownersDto;
    }

    public async Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        Owner owner = await this._ownerRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        OwnerDto ownerDto = owner.Adapt<OwnerDto>();

        return ownerDto;
    }

    public async Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default)
    {
        Owner owner = ownerForUpdateDto.Adapt<Owner>();

        owner.Id = ownerId;
        await this._ownerRepository.UpdateAsync(owner);
        await this._ownerRepository.SaveChangesAsync(cancellationToken);
    }
}