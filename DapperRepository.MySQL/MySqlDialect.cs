﻿using System.Collections.Generic;
using DapperRepository.Drapper.Sql;

namespace DapperRepository.MySQL
{
    public class MySqlDialect : SqlDialectBase
    {
        public override char OpenQuote
        {
            get { return '`'; }
        }

        public override char CloseQuote
        {
            get { return '`'; }
        }

        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LAST_INSERT_ID() AS Id";
        }

        public override string GetPagingSql(string sql, int page, int resultsPerPage,
            IDictionary<string, object> parameters)
        {
            int startValue = page*resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults,
            IDictionary<string, object> parameters)
        {
            string result = string.Format("{0} LIMIT @firstResult, @maxResults", sql);
            parameters.Add("@firstResult", firstResult);
            parameters.Add("@maxResults", maxResults);
            return result;
        }

        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            return base.GetColumnName(null, columnName, alias);
        }

        public override string GetTableName(string schemaName, string tableName, string alias)
        {
            return base.GetTableName(null, tableName, alias);
        }
    }
}