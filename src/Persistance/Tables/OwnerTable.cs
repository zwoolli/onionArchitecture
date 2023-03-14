using Domain.Entities;

namespace Persistance.Tables;
public class OwnerTable
{
    public OwnerTable(Owner owner)
    {
        this.owner_id = owner.Id;
        this.name = owner.Name;
        this.date_of_birth = owner.DateOfBirth;
        this.address = owner.Address;
        this.accounts = owner.Accounts.Select(a => new AccountTable(a)).ToList();
    }

    public Guid owner_id { get; set; }
    public string name { get; set; } = default!;
    public DateTime date_of_birth { get; set; }
    public string address { get; set; } = default!;
    public List<AccountTable> accounts { get; set; } = default!;

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