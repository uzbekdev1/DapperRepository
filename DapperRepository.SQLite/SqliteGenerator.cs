using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Sql;
using DapperRepository.Extensions;
using DapperRepository.Models;
using DapperRepository.Drapper.Mapper;

namespace DapperRepository.SQLite
{
    internal class SqliteGenerator : SqlGenerator
    {

        private static RepositoryModel _model;
        private const string DATABASE_KEY = @"Data Source";

        public SqliteGenerator(IDapperExtensionsConfiguration configuration)
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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);

            //create database if not exists
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(dbName);
            }

            return String.Empty;
        }

        public override string Exists(IClassMapper classMap)
        {
            return String.Format("SELECT COUNT(*) FROM sqlite_master WHERE name ='{0}' and type='table'", classMap.TableName);
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

            foreach (var columnKey in columnKeys)
            {
                var column = columns.FirstOrDefault(f => columnKey.Key == f.PropertyInfo.PropertyType &&
                    columnKey.Value.IndexOf(f.Name, StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (column == null)
                    continue;

                if (index > 0)
                    sql.Append(",");

                switch (column.KeyType)
                {
                    case KeyType.Identity:
                        sql.AppendFormat("{0} {1} NOT NULL PRIMARY KEY AUTOINCREMENT",
                             columnKey.Value, Configuration.DataMapper[columnKey.Key]);

                        break;
                    default:
                        sql.AppendFormat("{0} {1} NULL",
                                 columnKey.Value, Configuration.DataMapper[columnKey.Key]);
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
