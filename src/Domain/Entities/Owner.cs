namespace Domain.Entities;

public class Owner 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = default!;
    public ICollection<Account> Accounts { get; set; } = default!;
}