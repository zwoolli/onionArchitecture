namespace Domain.Entities;

public class Account 
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public string AccountType { get; set; } = default!;
    public Guid OwnerId { get; set; }
}