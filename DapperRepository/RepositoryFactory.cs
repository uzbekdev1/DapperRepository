using System;
using System.IO;
using DapperRepository.Demo.Helpers;
using DapperRepository.Enums;
using DapperRepository.Extensions;
using DapperRepository.Models;

namespace DapperRepository
{
    public static class RepositoryFactory
    {
        private static RepositoryModel _model;
        private static IDrapperManager _manager;

        private static void PreInit()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RepositorySettings.CONFIG_FILE);
            var doc = XLinqHelper.GetConfigDocument(path);
            var root = doc.Element("Repository");

            //model
            if (root == null)
            {
                _model = new RepositoryModel
                {
                    DriverType = RepositorySettings.DEFAULT_DRIVER.AsEnum<DriverType>(),
                    ConnectionString = RepositorySettings.DEFAULT_CONNECTION_STRING,
                    Options = new Options
                    {
                        CommandTimeOut = RepositorySettings.COMMAND_TIME_OUT,
                        MigrationsEnabled = true,
                        CreateDatabaseIfNotExists = true,
                        MigrationDataLossAllowed = false
                    }
                };
            }
            else
            {
                _model = new RepositoryModel
                {
                    DriverType = root.GetElementValue("DriverType").AsEnum<DriverType>(),
                    ConnectionString = root.GetElementValue("ConnectionString")
                };

                var options = root.Element("Options");

                if (options != null)
                {
                    _model.Options = new Options
                    {
                        CommandTimeOut = options.GetElementValue("CommandTimeOut").AsInt(),
                        DefaultSchema = options.GetElementValue("DefaultSchema"),
                        MigrationsEnabled = options.GetElementValue("MigrationsEnabled").AsBoolString(),
                        CreateDatabaseIfNotExists = options.GetElementValue("CreateDatabaseIfNotExists").AsBoolString(),
                        MigrationDataLossAllowed = options.GetElementValue("MigrationDataLossAllowed").AsBoolString()
                    };
                }
            }
        }

        static RepositoryFactory()
        {
            PreInit();
        }


        public static RepositoryModel GetModel()
        {
            return _model;
        }

        public static void SetManager(IDrapperManager manager)
        {
            _manager = manager;
        }

        public static IDrapperManager GetManager()
        {
            return _manager;
        }

    }
}
