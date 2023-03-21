using Domain.Entities;

namespace Persistance.Tables;

public class AccountTable
{
    public AccountTable() {}
    public AccountTable(Account account)
    {
        this.account_id = account.Id;
        this.owner_id = account.OwnerId;
        this.account_type = account.AccountType;
        this.date_created = account.DateCreated;
    }

    public static string TableName { get; } = nameof(Account);
    public Guid account_id { get; set; }
    public Guid owner_id { get; set; }
    public string account_type { get; set; } = default!;
    public DateTime date_created { get; set; }

    public class Column
    {
        public static string AccountId { get; } = nameof(AccountTable.account_id);
        public static string OwnerId { get; } = nameof(AccountTable.owner_id);
        public static string AccountType { get; } = nameof(AccountTable.account_type);
        public static string DateCreated { get; } = nameof(AccountTable.date_created);
    }

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