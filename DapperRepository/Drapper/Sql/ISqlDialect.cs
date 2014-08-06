using System.Collections.Generic;

namespace DapperRepository.Drapper.Sql
{
    public interface ISqlDialect
    {
        char OpenQuote { get; }
        char CloseQuote { get; }
        string BatchSeperator { get; }
        bool SupportsMultipleStatements { get; }
        char ParameterPrefix { get; }
        string EmptyExpression { get; }
        string GetTableName(string schemaName, string tableName, string alias);
        string GetColumnName(string prefix, string columnName, string alias);
        string GetIdentitySql(string tableName);
        string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);
        string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);
        bool IsQuoted(string value);
        string QuoteString(string value);
        string UnQuoteString(string value);
    }
}