using System;
using System.Collections.Generic;
using DapperRepository.Drapper;
using MySql.Data.MySqlClient;

namespace DapperRepository.MySQL
{
    public class MySqlManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;

        static MySqlManager()
        {
            _dataMapper = new Dictionary<Type, string>
            {
                {typeof (long), "bigint"},
                {typeof (int), "integer"},
                {typeof (byte), "smallint"},
                {typeof (string), "text"},
                {typeof (char), "varchar"},
                {typeof (bool), "bit(1)"},
                {typeof (DateTime), "datetime"},
                {typeof (float), "float"},
                {typeof (double), "double"},
                {typeof (decimal), "decimal"},
                {typeof (byte[]), "binary"},
            };
        }

        public MySqlManager(string connectionString)
            : base(new MySqlGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null, new MySqlDialect())),
                new MySqlConnection(connectionString),
                new MySqlDialect())
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