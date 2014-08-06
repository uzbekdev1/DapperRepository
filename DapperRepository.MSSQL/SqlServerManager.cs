using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DapperRepository.Drapper;

namespace DapperRepository.MSSQL
{
    public class SqlServerManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;

        static SqlServerManager()
        {
            _dataMapper = new Dictionary<Type, string>
            {
                {typeof (long), "BIGINT"},
                {typeof (int), "INT"},
                {typeof (byte), "SMALLINT"},
                {typeof (string), "NVARCHAR(MAX)"},
                {typeof (bool), "BIT"},
                {typeof (DateTime), "DATETIME"},
                {typeof (float), "FLOAT"},
                {typeof (double), "FLOAT"},
                {typeof (decimal), "NUMERIC"},
                {typeof (byte[]), "VARBINARY"},
            };
        }

        public SqlServerManager(string conectionString)
            : base(
                new SqlServerGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null, new SqlServerDialect())),
                new SqlConnection(conectionString),
                new SqlServerDialect())
        {
        }

        public override void Init()
        {
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
        }
    }
}