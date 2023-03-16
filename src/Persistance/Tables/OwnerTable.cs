using Domain.Entities;

namespace Persistance.Tables;

public class OwnerTable
{
    public OwnerTable() {}
    public OwnerTable(Owner owner)
    {
        this.owner_id = owner.Id;
        this.name = owner.Name;
        this.date_of_birth = owner.DateOfBirth;
        this.address = owner.Address;
        //https://stackoverflow.com/questions/46188867/tolist-argumentnullexception-handling
        // Tolist doesnt work on null object
        // Set owner.accounts.select... equal to variable and see what it is
        this.accounts = owner.Accounts.Select(a => new AccountTable(a)).ToList();
    }
    public static string Title { get; } = nameof(Owner);
    public Guid owner_id { get; set; }
    public string name { get; set; } = default!;
    public DateTime date_of_birth { get; set; }
    public string address { get; set; } = default!;
    public List<AccountTable> accounts { get; set; } = default!;

    public class Column
    {
        public static string OwnerId { get; } = nameof(OwnerTable.owner_id);
        public static string Name { get; } = nameof(OwnerTable.name);
        public static string DateOfBirth { get; } = nameof(OwnerTable.date_of_birth);
        public static string Address { get; } = nameof(OwnerTable.address);
        public static string Accounts { get; } = nameof(OwnerTable.accounts);
    }
    public Owner Adapt()
    {
        return new Owner
        {
            Id = this.owner_id,
            Name = this.name,
            DateOfBirth = this.date_of_birth,
            Address = this.address,
            Accounts = this.accounts.Select(a => a.Adapt()).ToList()
        };
    }
}