using Contracts;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/owners/{ownerId:guid}/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AccountsController(IServiceManager serviceManager) => this._serviceManager = serviceManager;

    [HttpGet]
    public async Task<IActionResult> GetAccounts(Guid ownerId, CancellationToken cancellationToken)
    {
        IEnumerable<AccountDto> accounts = await this._serviceManager.AccountService.GetAllByOwnerIdAsync(ownerId, cancellationToken);

        return Ok(accounts); 
    }

    [HttpGet("{accountId:guid}")]
    public async Task<IActionResult> GetAccountById(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
    {
        AccountDto account = await this._serviceManager.AccountService.GetByIdAsync(ownerId, accountId, cancellationToken);

        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(Guid ownerId, [FromBody] AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken)
    {
        AccountDto account = await this._serviceManager.AccountService.CreateAsync(ownerId, accountForCreationDto, cancellationToken);

        return CreatedAtAction(nameof(GetAccountById), new { ownerId = account.OwnerId, accountId = account.Id }, account);
    }

    [HttpDelete("{accountId:guid}")]
    public async Task<IActionResult> DeleteAccount(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
    {
        await this._serviceManager.AccountService.DeleteAsync(ownerId, accountId, cancellationToken);

        return NoContent();
    }
}