using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DapperRepository.Drapper;
using DapperRepository.Drapper.Mapper;
using DapperRepository.Drapper.Sql;
using DapperRepository.Extensions;
using DapperRepository.Models;

namespace DapperRepository.MSSQL
{
    internal class SqlServerGenerator : SqlGenerator
    {
        private static RepositoryModel _model;
        private const string DATABASE_KEY = @"Initial Catalog";
        private const string DATABASE_VALUE = "master";

        private object ExecuteScalar(string query)
        {
            object result = null;

            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();

                using (var com = new SqlCommand(query, conn))
                    result = com.ExecuteScalar();

                conn.Close();
            }

            return result;
        }

        public SqlServerGenerator(IDapperExtensionsConfiguration configuration)
            : base(configuration)
        {
            _model = RepositoryFactory.GetModel();
        }

        public override string Schema(IClassMapper classMapper)
        {
            return String.Format(@"IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{0}')) 
EXEC ('CREATE SCHEMA [{0}] AUTHORIZATION [dbo]');", classMapper.SchemaName);
        }

        public override string Database()
        {
            var connectionKeys = _model.ConnectionString.AsSplit(";");
            var dbKeys = connectionKeys.Select(item => item.AsSplit("="))
                .FirstOrDefault(f => f.Length == 2 && f[0].IsString(DATABASE_KEY));
            var dbName = dbKeys[1];
            var dataPath = ExecuteScalar(String.Format(@"SELECT REPLACE(physical_name,name,N'{0}') FROM sys.master_files WHERE database_id = DB_ID(N'master') AND physical_name like '%.mdf';", dbName)).AsString();
            var logPath = ExecuteScalar(String.Format(@"SELECT REPLACE(physical_name,name,N'{0}_log') FROM sys.master_files WHERE database_id = DB_ID(N'master') AND physical_name like '%.ldf';", dbName)).AsString();

            return String.Format(@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{0}')
             BEGIN  
              CREATE DATABASE [{0}] ON  PRIMARY 
	            (
	             NAME = N'{0}', 
	             FILENAME =N'{1}', 
	             SIZE = 31744KB , 
	             MAXSIZE = UNLIMITED, 
	             FILEGROWTH = 1024KB 
	             )
             LOG ON 
	            (
	              NAME = N'{0}_log', 
	              FILENAME =N'{2}' ,
	              SIZE = 3840KB , 
	              MAXSIZE = 2048GB , 
	              FILEGROWTH = 10%
	             )
            END", dbName, dataPath, logPath);
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

            sql.AppendFormat(@"IF (OBJECT_ID('{0}', 'U') IS NULL)", GetTableName(classMap))
                .AppendLine();
            sql.AppendFormat("CREATE TABLE {0}", GetTableName(classMap)).AppendLine();
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
