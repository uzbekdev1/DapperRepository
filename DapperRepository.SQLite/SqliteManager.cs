using System;
using System.Collections.Generic;
using System.Data.SQLite;
using DapperRepository.Drapper;
using DapperRepository.Models;

namespace DapperRepository.SQLite
{
    internal class SqliteManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;
        private RepositoryModel _model;
        static SqliteManager()
        {
            _dataMapper = new Dictionary<Type, string>
            {
                {typeof (long), "INTEGER"},
                {typeof (int), "INTEGER"},
                {typeof (byte), "INTEGER"},
                {typeof (string), "TEXT"},
                {typeof (char), "TEXT"},
                {typeof (bool), "BIT"},
                {typeof (DateTime), "DATETIME"},
                {typeof (float), "REAL"},
                {typeof (double), "DOUBLE"},
                {typeof (decimal), "NUMERIC"},
                {typeof (byte[]), "BLOB"},
            };
        }

        public SqliteManager(RepositoryModel model)
            : base(new SqliteGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null, new SqliteDialect())),
                new SQLiteConnection(model.ConnectionString),
                new SqliteDialect())
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
