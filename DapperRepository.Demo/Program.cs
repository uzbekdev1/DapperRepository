using System.Configuration;
using DapperRepository.Helpers;
using DapperRepository.MSSQL;

namespace DapperRepository.Demo
{
    internal class Program
    {
        static Program()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            RepositoryHelper.ConfigureRepository(new SqlServerManager(connectionString));
            MigrationHelper.Configure<TempModel>();
        }

        private static void Main(string[] args)
        {
        }
    }
}