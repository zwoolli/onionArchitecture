namespace Contracts;

public class OwnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = default!;
    public IEnumerable<AccountDto>? Accounts { get; set; }
}