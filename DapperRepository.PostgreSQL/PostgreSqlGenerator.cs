using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;

namespace DapperRepository.PostgreSQL
{
    public class PostgreSqlGenerator : SqlGenerator
    {
        public PostgreSqlGenerator(IDapperExtensionsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Schema(IClassMapper classMapper)
        {
            return String.Format("CREATE SCHEMA IF NOT EXISTS \"{0}\"", classMapper.SchemaName);
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

            sql.AppendFormat("CREATE TABLE  IF NOT EXISTS {0}", GetTableName(classMap)).AppendLine();
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
                    sql.AppendLine(",");

                switch (column.KeyType)
                {
                    case KeyType.Identity:
                        sql.AppendFormat("{0} SERIAL NOT NULL PRIMARY KEY",
                            columnKey.Value);
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