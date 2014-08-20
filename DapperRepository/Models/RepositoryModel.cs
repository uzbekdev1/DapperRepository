using DapperRepository.Enums;

namespace DapperRepository.Models
{ 

    public struct Options
    {
        public int CommandTimeOut { get; set; }

        public string DefaultSchema { get; set; }

        public bool MigrationsEnabled { get; set; }

        public bool CreateDatabaseIfNotExists { get; set; }

        public bool MigrationDataLossAllowed { get; set; }

    }

    public struct RepositoryModel
    {

        public DriverType DriverType { get; set; }

        public string ConnectionString { get; set; }

        public Options Options { get; set; }
          
    }
}
