using System.Reflection;

namespace DapperRepository.Drapper.Mapper
{
    /// <summary>
    ///     Maps an entity property to its corresponding column in the database.
    /// </summary>
    public interface IPropertyMap
    {
        string Name { get; }
        string ColumnName { get; }
        bool Ignored { get; }
        bool IsReadOnly { get; }
        KeyType KeyType { get; }
        PropertyInfo PropertyInfo { get; }
    }
}