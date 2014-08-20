using System;
using DapperRepository.Demo;
using DapperRepository.Models;

namespace DapperRepository.Helpers
{
    public static class MigrationHelper
    {
        private static IDrapperManager _manager;
        private static RepositoryModel _model;

        private static void Init()
        {
            if (_manager == null)
                _manager = RepositoryFactory.GetManager();

            _model = RepositoryFactory.GetModel();
        }

        public static void Configure<T>() where T : class
        {
            Init();

            if (!_model.Options.MigrationsEnabled)
                return;

            var commandTimeOut = _model.Options.CommandTimeOut;

            if (_model.Options.CreateDatabaseIfNotExists)
                _manager.Database.CreateDatabaseIfNotExists(_model.ConnectionString, commandTimeOut);

            _manager.Database.Open();

            if (_manager.Database.Exists<T>(commandTimeOut))
            {
                if (_model.Options.MigrationDataLossAllowed)
                    _manager.Database.Drop<T>(commandTimeOut);
            }

            if (!String.IsNullOrWhiteSpace(_model.Options.DefaultSchema))
                _manager.Database.CreateSchemaIfNotExists<T>(commandTimeOut);

            _manager.Database.Create<T>(commandTimeOut);

            _manager.Database.Close();
        }
    }
}
