using System.Data;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository
{
    public abstract class DrapperManager : IDrapperManager
    {
        protected DrapperManager()
        {
            //TODO:    

            Init();
        }

        protected DrapperManager(ISqlGenerator generator, IDbConnection dbConnection, ISqlDialect dialect)
        {
            Dialect = dialect;
            Generator = generator;
            DbConnection = dbConnection;
            Database = new Database(dbConnection, generator);

            Init();
        }

        public ISqlGenerator Generator { get; private set; }

        public IDbConnection DbConnection { get; private set; }

        public IDatabase Database { get; private set; }

        public ISqlDialect Dialect { get; private set; }

        public abstract void Dispose();
        public abstract void Init();
    }
}