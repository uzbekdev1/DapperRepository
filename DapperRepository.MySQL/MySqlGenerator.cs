using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;
using DapperRepository.Extensions;
using DapperRepository.Models;

namespace DapperRepository.MySQL
{
    internal class MySqlGenerator : SqlGenerator
    {

        private static RepositoryModel _model;
        private const string DATABASE_KEY = @"database";

        public MySqlGenerator(IDapperExtensionsConfiguration configuration)
            : base(configuration)
        {
            _model = RepositoryFactory.GetModel();
        }

        public override string Database()
        {
            var connectionKeys = _model.ConnectionString.AsSplit(";");
            var dbKeys = connectionKeys.Select(item => item.AsSplit("="))
                .FirstOrDefault(f => f.Length == 2 && f[0].IsString(DATABASE_KEY));
            var dbName = dbKeys[1];

            return String.Format("CREATE DATABASE IF NOT EXISTS `{0}`", dbName);
        }

        public override string ConnectionString()
        {
            var connectionKeys = _model.ConnectionString.AsSplit(";");
            var dbKeys = connectionKeys.Select(item => item.AsSplit("="))
                .Where(f => f.Length == 2 && !f[0].IsString(DATABASE_KEY));

            return String.Join("", dbKeys.Select(s => String.Format("{0}={1};", s[0], s[1])));
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

            sql.AppendFormat("CREATE TABLE IF NOT EXISTS {0}", GetTableName(classMap)).AppendLine();
            sql.AppendLine("(");

            var index = 0;
            var pk = String.Empty;

            foreach (var columnKey in columnKeys)
            {
                var column = columns.FirstOrDefault(f => columnKey.Key == f.PropertyInfo.PropertyType &&
                    columnKey.Value.IndexOf(f.Name, StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (column == null)
                    continue;

                switch (column.KeyType)
                {
                    case KeyType.Identity:
                        pk = columnKey.Value;
                        sql.AppendFormat("{0} {1} NOT NULL AUTO_INCREMENT",
                            pk, Configuration.DataMapper[columnKey.Key]);

                        break;
                    default:
                        sql.AppendFormat("{0} {1} NULL",
                                 columnKey.Value, Configuration.DataMapper[columnKey.Key]);
                        break;
                }

                sql.Append(",")
                    .AppendLine();

                index++;
            }

            if (!String.IsNullOrWhiteSpace(pk))
            {
                sql.AppendFormat(" PRIMARY KEY ({0})", pk)
                    .AppendLine();
            }

            sql.AppendLine(")");

            return sql.ToString();
        }

    }
}
