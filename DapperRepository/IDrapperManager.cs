using System;
using System.Data;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository
{
    public interface IDrapperManager : IDisposable
    {
        ISqlGenerator Generator { get; }

        IDbConnection DbConnection { get; }

        IDatabase Database { get; }

        ISqlDialect Dialect { get; }
    }
}