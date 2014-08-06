namespace DapperRepository.Helpers
{
    public static class RepositoryHelper
    {
        private static IDrapperManager _manager;

        public static void ConfigureRepository(IDrapperManager manager)
        {
            _manager = manager;
        }

        public static IDrapperManager GetManager()
        {
            return _manager;
        }
    }
}