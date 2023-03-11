using System.Data.Common;

namespace Domain.Repositories;

public interface IDBConnector
{
    Task<DbTransaction> Transaction();
}