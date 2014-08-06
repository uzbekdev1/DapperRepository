using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository.Drapper
{
    public sealed class DapperExtensionsConfiguration : IDapperExtensionsConfiguration
    {
        private readonly ConcurrentDictionary<Type, IClassMapper> _classMaps =
            new ConcurrentDictionary<Type, IClassMapper>();

        public DapperExtensionsConfiguration(Type defaultMapper, IList<Assembly> mappingAssemblies,
            ISqlDialect sqlDialect)
            : this(new Dictionary<Type, string>(), defaultMapper, mappingAssemblies, sqlDialect)
        {
        }

        public DapperExtensionsConfiguration(IDictionary<Type, String> dataMapper, Type defaultMapper,
            IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            DefaultMapper = defaultMapper;
            MappingAssemblies = mappingAssemblies ?? new List<Assembly>();
            Dialect = sqlDialect;
            DataMapper = dataMapper;
        }

        public IDictionary<Type, string> DataMapper { get; private set; }
        public Type DefaultMapper { get; private set; }
        public IList<Assembly> MappingAssemblies { get; private set; }
        public ISqlDialect Dialect { get; private set; }

        public IClassMapper GetMap(Type entityType)
        {
            IClassMapper map;
            if (!_classMaps.TryGetValue(entityType, out map))
            {
                Type mapType = GetMapType(entityType);
                if (mapType == null)
                {
                    mapType = DefaultMapper.MakeGenericType(entityType);
                }

                map = Activator.CreateInstance(mapType) as IClassMapper;
                _classMaps[entityType] = map;
            }

            return map;
        }

        public IClassMapper GetMap<T>() where T : class
        {
            return GetMap(typeof (T));
        }

        public void ClearCache()
        {
            _classMaps.Clear();
        }

        public Guid GetNextGuid()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            var dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            var timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long) (timeOfDay.TotalMilliseconds/3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }

        private Type GetMapType(Type entityType)
        {
            Func<Assembly, Type> getType = a =>
            {
                Type[] types = a.GetTypes();
                return (from type in types
                    let interfaceType = type.GetInterface(typeof (IClassMapper<>).FullName)
                    where
                        interfaceType != null &&
                        interfaceType.GetGenericArguments()[0] == entityType
                    select type).SingleOrDefault();
            };

            Type result = getType(entityType.Assembly);
            if (result != null)
            {
                return result;
            }

            foreach (Assembly mappingAssembly in MappingAssemblies)
            {
                result = getType(mappingAssembly);
                if (result != null)
                {
                    return result;
                }
            }

            return getType(entityType.Assembly);
        }
    }
}