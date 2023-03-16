namespace Domain.Repositories;

public interface IDbConfiguration
{
    public string ConnectionString { get; }
}