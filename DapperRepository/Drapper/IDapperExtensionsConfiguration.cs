using System;
using System.Collections.Generic;
using System.Reflection;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository.Drapper
{
    public interface IDapperExtensionsConfiguration
    {
        IDictionary<Type, string> DataMapper { get; }
        Type DefaultMapper { get; }
        IList<Assembly> MappingAssemblies { get; }
        ISqlDialect Dialect { get; }
        IClassMapper GetMap(Type entityType);
        IClassMapper GetMap<T>() where T : class;
        void ClearCache();
        Guid GetNextGuid();
    }
}