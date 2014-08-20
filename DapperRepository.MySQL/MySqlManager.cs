using System;
using System.Collections.Generic;
using DapperRepository.Drapper;
using MySql.Data.MySqlClient;
using DapperRepository.Models;

namespace DapperRepository.MySQL
{
    internal class MySqlManager : DrapperManager
    {
        private static readonly IDictionary<Type, string> _dataMapper;
        private RepositoryModel _model;
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

        public MySqlManager(RepositoryModel model)
            : base(new MySqlGenerator(new DapperExtensionsConfiguration(_dataMapper, null, null, new MySqlDialect())),
                new MySqlConnection(model.ConnectionString),
                new MySqlDialect())
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
