namespace DapperRepository.Helpers
{
    public static class MigrationHelper
    {
        private static IDrapperManager _manager;

        public static void Configure<T>(bool allowDroped = false) where T : class
        {
            if (_manager == null)
                _manager = RepositoryHelper.GetManager();

            if (allowDroped &&
                _manager.Database.Exists<T>())
                _manager.Database.Drop<T>();

            _manager.Database.Create<T>();
        }
    }
}