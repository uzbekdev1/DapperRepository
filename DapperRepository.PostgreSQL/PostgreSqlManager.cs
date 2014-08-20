using System;
using System.Collections.Generic;
using DapperRepository.Drapper;
using DapperRepository.Models;
using Npgsql;

namespace DapperRepository.PostgreSQL
{
    internal class PostgreSqlManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;
        private readonly RepositoryModel _model;

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

        public PostgreSqlManager(RepositoryModel model)
            : base(new PostgreSqlGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null, new PostgreSqlDialect())),
                new NpgsqlConnection(model.ConnectionString),
                new PostgreSqlDialect())
        {
            _model = model; 
        }

        public override void Init()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {

        }
    }
}
