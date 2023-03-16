using Domain.Repositories;

namespace Web.Settings;
public class DbConfiguration : IDbConfiguration
{
    private readonly string _connectionString;

    public DbConfiguration(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("OnionArchitecture");

        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

        this._connectionString = connectionString;
    }
    public string ConnectionString => this._connectionString;
}