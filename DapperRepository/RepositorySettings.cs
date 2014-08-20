namespace DapperRepository
{
    internal static class RepositorySettings
    {
        //RepositoryModel config file
        public const string CONFIG_FILE = @"RepositoryConfig.xml";
         
        //database
        public const string DEFAULT_DRIVER = @"SQLite";
        public const string DEFAULT_CONNECTION_STRING = @"Data Source=QueryProcessorStorage.sqlite;Version=3;";
        public const int COMMAND_TIME_OUT = 3600;
    }
}
