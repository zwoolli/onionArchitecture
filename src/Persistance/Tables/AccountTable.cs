using Domain.Entities;

namespace Persistance.Tables;

public class AccountTable
{
    public AccountTable(Account account)
    {
        this.account_id = account.Id;
        this.owner_id = account.OwnerId;
        this.account_type = account.AccountType;
        this.date_created = account.DateCreated;
    }

    public Guid account_id { get; set; }
    public Guid owner_id { get; set; }
    public string account_type { get; set; } = default!;
    public DateTime date_created { get; set; }

    public Account Adapt()
    {
        return new Account
        {
            Id = this.account_id,
            OwnerId = this.owner_id,
            AccountType = this.account_type,
            DateCreated = this.date_created
        };
    }
}