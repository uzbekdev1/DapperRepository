using System.Collections.Generic;
using DapperRepository.Drapper.Mapper;

namespace DapperRepository.Drapper.Sql
{
    public interface ISqlGenerator
    {
        IDapperExtensionsConfiguration Configuration { get; }

        string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort,
            IDictionary<string, object> parameters);

        string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage,
            IDictionary<string, object> parameters);

        string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults,
            IDictionary<string, object> parameters);

        string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string Schema(IClassMapper classMapper);
        string Create(IClassMapper classMap);
        string Exists(IClassMapper classMap);
        string Drop(IClassMapper classMap);
        string Insert(IClassMapper classMap);
        string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);
        string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string IdentitySql(IClassMapper classMap);
        string GetTableName(IClassMapper map);
        string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias);
        string GetColumnName(IClassMapper map, string propertyName, bool includeAlias);
        bool SupportsMultipleStatements();
        string BuildSelectColumns(IClassMapper classMap);
    }
}