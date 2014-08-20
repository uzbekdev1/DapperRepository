using System;
using System.Reflection;
using DapperRepository.Demo.Helpers;
using DapperRepository.Helpers;

namespace DapperRepository.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Serializable]
    public class MigrationAttribute : Attribute
    {
        private static Type _migrationType;

        private static void DoMigrate()
        {
            var migrationHelperType = typeof(MigrationHelper);
            var configureMethodInfo = migrationHelperType.GetMethod("Configure", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var configureWithType = configureMethodInfo.MakeGenericMethod(new[] { _migrationType });
            
            configureWithType.Invoke(migrationHelperType, new object[] { });
        }

        public MigrationAttribute(Type migrationType)
        {
            _migrationType = migrationType;

            DoMigrate();
        }

    }
}
