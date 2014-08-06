using System;
using System.Collections.Generic;
using DapperRepository.Drapper;
using Npgsql;

namespace DapperRepository.PostgreSQL
{
    public class PostgreSqlManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;

        static PostgreSqlManager()
        {
            _dataMapper = new Dictionary<Type, string>
            {
                {typeof (long), "bigint"},
                {typeof (int), "integer"},
                {typeof (byte), "smallint"},
                {typeof (string), "text"},
                {typeof (char), "character varying"},
                {typeof (bool), "boolean"},
                {typeof (DateTime), "timestamp without time zone"},
                {typeof (float), "real"},
                {typeof (double), "double precision"},
                {typeof (decimal), "numeric"},
                {typeof (byte[]), "bytea"},
            };
        }

        public PostgreSqlManager(string connectionString)
            : base(
                new PostgreSqlGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null,
                    new PostgreSqlDialect())),
                new NpgsqlConnection(connectionString),
                new PostgreSqlDialect())
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