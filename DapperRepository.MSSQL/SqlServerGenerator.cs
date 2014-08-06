using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository.MSSQL
{
    public class SqlServerGenerator : SqlGenerator
    {
        public SqlServerGenerator(IDapperExtensionsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Schema(IClassMapper classMapper)
        {
            return String.Format(@"IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{0}')) 
EXEC ('CREATE SCHEMA [{0}] AUTHORIZATION [dbo]');", classMapper.SchemaName);
        }

        public override string Create(IClassMapper classMap)
        {
            IPropertyMap[] columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly)).ToArray();

            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            List<KeyValuePair<Type, string>> columnKeys =
                columns.Select(
                    p => new KeyValuePair<Type, string>(p.PropertyInfo.PropertyType, GetColumnName(classMap, p, false)))
                    .ToList();
            var sql = new StringBuilder();

            sql.AppendFormat(@"IF (OBJECT_ID('{0}', 'U') IS NULL)", GetTableName(classMap))
                .AppendLine();
            sql.AppendFormat("CREATE TABLE {0}", GetTableName(classMap)).AppendLine();
            sql.AppendLine("(");

            int index = 0;
            foreach (var columnKey in columnKeys)
            {
                IPropertyMap column = columns.FirstOrDefault(f => columnKey.Key == f.PropertyInfo.PropertyType &&
                                                                  columnKey.Value.IndexOf(f.Name,
                                                                      StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (column == null)
                    continue;

                if (index > 0)
                    sql.Append(",");

                switch (column.KeyType)
                {
                    case KeyType.Identity:
                        sql.AppendFormat("{0} {1} NOT NULL IDENTITY(1,1) PRIMARY KEY",
                            columnKey.Value,
                            Configuration.DataMapper[columnKey.Key]);
                        break;
                    default:
                        sql.AppendFormat("{0} {1} NULL",
                            columnKey.Value,
                            Configuration.DataMapper[columnKey.Key]);
                        break;
                }

                sql.AppendLine();
                index++;
            }

            sql.AppendLine(")");

            return sql.ToString();
        }
    }
}