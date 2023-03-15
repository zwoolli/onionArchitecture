using Contracts;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public OwnersController(IServiceManager serviceManager) => this._serviceManager = serviceManager;

    [HttpGet]
    public async Task<IActionResult> GetOwners(CancellationToken cancellationToken)
    {
        IEnumerable<OwnerDto> owners = await this._serviceManager.OwnerService.GetAllAsync(cancellationToken);

        return Ok(owners);
    }

    [HttpGet("{ownerId:guid}")]
    public async Task<IActionResult> GetOwnerById(Guid ownerId, CancellationToken cancellationToken)
    {
        OwnerDto owner = await this._serviceManager.OwnerService.GetByIdAsync(ownerId, cancellationToken);

        return Ok(owner);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreationDto ownerForCreationDto)
    {
        OwnerDto owner = await this._serviceManager.OwnerService.CreateAsync(ownerForCreationDto);

        return CreatedAtAction(nameof(GetOwnerById), new { ownerId = owner.Id }, owner);
    }

    [HttpPut("{ownerId:guid}")]
    public async Task<IActionResult> UpdateOwner(Guid ownerId, [FromBody] OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken)
    {
        await this._serviceManager.OwnerService.UpdateAsync(ownerId, ownerForUpdateDto, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{ownerId:guid}")]
    public async Task<IActionResult> DeleteOwner(Guid ownerId, CancellationToken cancellationToken)
    {
        await this._serviceManager.OwnerService.DeleteAsync(ownerId, cancellationToken);

        return NoContent();
    }
}