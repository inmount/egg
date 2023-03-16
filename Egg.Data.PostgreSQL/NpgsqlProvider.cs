using Egg.Data.Extensions;
using Egg.Data.PostgreSQL.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Egg.Data.PostgreSQL
{
    /// <summary>
    /// PostgreSQL数据库语法供应器
    /// </summary>
    public class NpgsqlProvider : IDatabaseProvider
    {
        /// <summary>
        /// PostgreSQL数据库语法供应器
        /// </summary>
        public NpgsqlProvider() { }

        /// <summary>
        /// 获取事务开始语句
        /// </summary>
        /// <returns></returns>
        public string GetTransactionBeginString() => "BEGIN TRANSACTION;";

        /// <summary>
        /// 获取事务结束语句
        /// </summary>
        /// <returns></returns>
        public string GetTransactionEndString() => "END TRANSACTION;";

        /// <summary>
        /// 获取名称定义字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetNameString(string name)
            => $"\"{name}\"";

        /// <summary>
        /// 获取值字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetValueString(string value)
            => $"'{value.Replace("'", "''")}'";

        /// <summary>
        /// 获取列创建字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetColumnCreateSqlString(string tableName, PropertyInfo column)
        {
            if (column.IsKey())
            {
                return $"{GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} NOT NULL";
            }
            else
            {
                return $"{GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} {(column.IsNullable() ? "NULL" : "NOT NULL")}";
            }
        }

        /// <summary>
        /// 获取完整的表名称
        /// </summary>
        /// <returns></returns>
        public string GetFullTableName<T>()
        {
            // 创建表
            Type type = typeof(T);
            string schema = type.GetSchemaName() ?? string.Empty;
            string name = GetNameString(type.GetTableName());
            if (schema.IsNullOrWhiteSpace()) return name;
            return GetNameString(schema) + "." + name;
        }

        /// <summary>
        /// 获取确保数据库创建语句
        /// </summary>
        /// <returns></returns>
        public string GetTableCreateSqlString<T>()
        {
            // 申明拼接字符串
            StringBuilder sb = new StringBuilder();
            // 创建表
            Type type = typeof(T);
            string tableName = type.GetTableName();
            string fullTableName = GetFullTableName<T>();
            // 拼接语句
            sb.AppendLine($"CREATE TABLE {fullTableName}(");
            // 获取所有字段
            var columns = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            bool isFirst = true;
            string keyName = string.Empty;
            foreach (var column in columns)
            {
                if (isFirst) { isFirst = false; } else { sb.Append(','); sb.AppendLine(); }
                sb.Append(new string(' ', 4));
                sb.AppendLine(GetColumnCreateSqlString(tableName, column));
                // 记录主键
                if (column.IsKey()) keyName = column.GetColumnName();
            }
            // 添加主键
            if (!keyName.IsNullOrWhiteSpace())
            {
                sb.Append(','); sb.AppendLine();
                sb.Append(new string(' ', 4));
                sb.Append($"CONSTRAINT \"PK_{tableName}\" PRIMARY KEY ({keyName})");
            }
            sb.AppendLine();
            sb.AppendLine(");");
            // 输出索引信息
            foreach (var column in columns)
            {
                if (column.IsUniqueIndex())
                {
                    sb.AppendLine($"CREATE UNIQUE INDEX {column.GetIndexName<T>()} ON {fullTableName} USING btree ({column.GetColumnName()});");
                }
                else if (column.IsIndex())
                {
                    sb.AppendLine($"CREATE INDEX {column.GetIndexName<T>()} ON {fullTableName} ({column.GetColumnName()});");
                }
            }
            // 输出描述信息
            foreach (var column in columns)
            {
                string description = column.GetDescription();
                if (!description.IsNullOrWhiteSpace()) sb.AppendLine($"COMMENT ON COLUMN {fullTableName}.{GetNameString(column.GetColumnName())} IS '{description}';");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取列添加字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetColumnAddSqlString<T>(PropertyInfo column)
        {
            // 申明拼接字符串
            StringBuilder sb = new StringBuilder();
            string fullTableName = GetFullTableName<T>();
            sb.AppendLine($"ALTER TABLE {fullTableName} ADD {GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} {(column.IsNullable() ? "NULL" : "NOT NULL")};");
            string description = column.GetDescription();
            if (!description.IsNullOrWhiteSpace()) sb.AppendLine($"COMMENT ON COLUMN {fullTableName}.{GetNameString(column.GetColumnName())} IS '{description}';");
            return sb.ToString();
        }

        /// <summary>
        /// 获取判断列是否存在语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public string GetColumnExistsSqlString<T>(PropertyInfo property)
        {
            Type type = typeof(T);
            string schema = type.GetSchemaName() ?? "public";
            string tableName = type.GetTableName();
            return $"select column_name from information_schema.columns WHERE table_schema = '{schema}' and table_name = '{tableName}' and column_name = '{property.GetColumnName()}';";
        }

        /// <summary>
        /// 获取数据库基础连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDatabaseConnectionBase GetDatabaseConnection(string connectionString)
            => new NpgsqlConnectionBase(connectionString);

        public string GetFuncString(string name, object[]? args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取判断表是否存在语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableExistsSqlString<T>()
        {
            Type type = typeof(T);
            string schema = type.GetSchemaName() ?? "public";
            string tableName = type.GetTableName();
            return $"select tablename from pg_tables where schemaname = '{schema}' and tablename = '{tableName}';";
        }

        /// <summary>
        /// 获取数据库检测脚本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDatabaseExistsSqlString(string name)
            => $"SELECT pd.datname FROM pg_catalog.pg_database pd WHERE pd.datname = {GetValueString(name)};";

        /// <summary>
        /// 获取数据库新增脚本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDatabaseCreateSqlString(string name)
            => $"CREATE DATABASE {GetNameString(name)};";

        /// <summary>
        /// 获取分库检测脚本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSchemaExistsSqlString(string name)
            => $"SELECT pn.nspname FROM pg_catalog.pg_namespace pn WHERE pn.nspname = {GetValueString(name)};";

        /// <summary>
        /// 获取分库创建脚本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSchemaCreateSqlString(string name)
            => $"CREATE SCHEMA {GetNameString(name)};";

        /// <summary>
        /// 获取数据库查询脚本
        /// </summary>
        /// <returns></returns>
        public string GetDatabasesQuerySqlString()
            => $"SELECT pd.datname FROM pg_catalog.pg_database pd;";

        /// <summary>
        /// 获取分库查询脚本
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string GetSchemasQuerySqlString(string dbName, string userName)
            => $"select s.schema_name from information_schema.schemata s WHERE s.schema_owner = {GetValueString(userName)} order by s.schema_name;";

        /// <summary>
        /// 获取表查询脚本
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public string GetTablesQuerySqlString(string dbName, string? schema = null)
            => $"select pt.tablename from pg_catalog.pg_tables pt where pt.schemaname = {GetValueString(schema ?? "public")} order by pt.tablename;";

        /// <summary>
        /// 获取表字段查询脚本
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public string GetColumnsQuerySqlString(string dbName, string tabName)
            => $"select c.column_name from information_schema.columns c WHERE c.table_schema = 'public' and c.table_name = {GetValueString(tabName)} ORDER BY c.column_name;";

        /// <summary>
        /// 获取表字段查询脚本
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="schema"></param>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public string GetColumnsQuerySqlString(string dbName, string schema, string tabName)
            => $"select c.column_name from information_schema.columns c WHERE c.table_schema = {GetValueString(schema)} and c.table_name = {GetValueString(tabName)} ORDER BY c.column_name;";
    }
}
