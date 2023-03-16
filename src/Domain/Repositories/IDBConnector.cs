using System.Data.Common;

namespace Domain.Repositories;

public interface IDbConnector
{
    Task<DbTransaction> Transaction();
}