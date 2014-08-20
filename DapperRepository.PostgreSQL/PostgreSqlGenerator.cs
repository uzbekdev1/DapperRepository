using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;
using DapperRepository.Extensions;
using DapperRepository.Models;

namespace DapperRepository.PostgreSQL
{
    internal class PostgreSqlGenerator : SqlGenerator
    {
        private static RepositoryModel _model;
        private const string DATABASE_KEY = @"Database";
        private const string DATABASE_VALUE = "postgres";

        public PostgreSqlGenerator(IDapperExtensionsConfiguration configuration)
            : base(configuration)
        {
            _model = RepositoryFactory.GetModel();
        }

        public override string Schema(IClassMapper classMapper)
        {
            return String.Format("CREATE SCHEMA IF NOT EXISTS \"{0}\"", classMapper.SchemaName);
        }


        public override string Database()
        {
            var connectionKeys = _model.ConnectionString.AsSplit(";");
            var dbKeys = connectionKeys.Select(item => item.AsSplit("="))
                .FirstOrDefault(f => f.Length == 2 && f[0].IsString(DATABASE_KEY));
            var dbName = dbKeys[1];

            return String.Format("CREATE DATABASE \"{0}\"" +
                                 @"WITH OWNER = postgres
                                           ENCODING = 'UTF8'
                                           TABLESPACE = pg_default;", dbName);
        }

        public override string ConnectionString()
        {
            var connectionKeys = _model.ConnectionString.AsSplit(";");
            var dbKeys = connectionKeys.Select(item => item.AsSplit("="))
                .Where(f => f.Length == 2);

            return String.Join("", dbKeys.Select(s => String.Format("{0}={1};", s[0], s[0].IsString(DATABASE_KEY) ? DATABASE_VALUE : s[1])));
        }

        public override string Create(IClassMapper classMap)
        {
            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly)).ToArray();

            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var columnKeys = columns.Select(p => new KeyValuePair<Type, string>(p.PropertyInfo.PropertyType, GetColumnName(classMap, p, false)))
                     .ToList();
            var sql = new StringBuilder();

            sql.AppendFormat("CREATE TABLE  IF NOT EXISTS {0}", GetTableName(classMap)).AppendLine();
            sql.AppendLine("(");

            var index = 0;
            foreach (var columnKey in columnKeys)
            {
                var column = columns.FirstOrDefault(f => columnKey.Key == f.PropertyInfo.PropertyType &&
                  columnKey.Value.IndexOf(f.Name, StringComparison.InvariantCultureIgnoreCase) >= 0);

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
