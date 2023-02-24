using Egg.Data.Entities;
using Egg.Data.Extensions;
using Egg.Data.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data.Sqlite
{
    /// <summary>
    /// PostgreSQL数据库语法供应器
    /// </summary>
    public class SqliteProvider : IDatabaseProvider
    {
        /// <summary>
        /// PostgreSQL数据库语法供应器
        /// </summary>
        public SqliteProvider() { }

        /// <summary>
        /// 获取数据库基础连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public IDatabaseConnectionBase GetDatabaseConnection(string connectionString)
            => new SqliteConnectionBase(connectionString);

        /// <summary>
        /// 获取事务开始语句
        /// </summary>
        /// <returns></returns>
        public string GetTransactionBeginString()
        {
            return "BEGIN TRANSACTION;";
        }

        /// <summary>
        /// 获取事务结束语句
        /// </summary>
        /// <returns></returns>
        public string GetTransactionEndString()
        {
            return "END TRANSACTION;";
        }

        /// <summary>
        /// 获取函数定义字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetFuncString(string name, object[]? args)
        {
            //throw new NotImplementedException();
            return "";
        }

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
                if (column.IsAutoIncrement())
                {
                    return $"{GetNameString(column.GetColumnName())} INTEGER PRIMARY KEY AUTOINCREMENT";
                }
                else
                {
                    return $"{GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} NOT NULL PRIMARY KEY";
                }
            }
            else
            {
                return $"{GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} {(column.IsNullable() ? "NULL" : "NOT NULL")}";
            }
        }

        /// <summary>
        /// 获取列添加字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetColumnAddSqlString(string tableName, PropertyInfo column)
        {
            return $"ALTER TABLE {GetNameString(tableName)} ADD {GetNameString(column.GetColumnName())} {column.GetColumnAttributeType()} {(column.IsNullable() ? "NULL" : "NOT NULL")};";
        }

        /// <summary>
        /// 获取确保数据库创建语句
        /// </summary>
        /// <returns></returns>
        public string GetTableExistsSqlString<T>()
        {
            // 创建表
            Type type = typeof(T);
            string tableName = type.Name;
            return $"SELECT [name] FROM [sqlite_master] WHERE [name] = '{tableName}' AND [type]='table' LIMIT 1;";
        }

        /// <summary>
        /// 获取确保数据库创建语句
        /// </summary>
        /// <returns></returns>
        public string GetColumnExistsSqlString(string tableName, PropertyInfo property)
        {
            // 获取字段信息
            string columnName = property.GetColumnName();
            return $"SELECT [name] FROM [sqlite_master] WHERE [type]='table'" +
                $" AND [name] ='{tableName}'" +
                $" AND ([sql] LIKE '%[{columnName}]%'" +
                $" OR [sql] LIKE '%\"{columnName}\"%'" +
                $" OR [sql] LIKE '%,{columnName},%'" +
                $" OR [sql] LIKE '%({columnName},%'" +
                $" OR [sql] LIKE '%,{columnName})%'" +
                $" OR [sql] LIKE '% {columnName} %'" +
                $") LIMIT 1;";
        }

        /// <summary>
        /// 获取完整的表名称
        /// </summary>
        /// <returns></returns>
        public string GetFullTableName<T>()
        {
            // 创建表
            Type type = typeof(T);
            return GetNameString(type.GetTableName());
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
            foreach (var column in columns)
            {
                if (isFirst) { isFirst = false; } else { sb.Append(','); sb.AppendLine(); }
                sb.Append(new string(' ', 4));
                sb.AppendLine(GetColumnCreateSqlString(tableName, column));
            }
            sb.AppendLine();
            sb.AppendLine(");");
            return sb.ToString();
        }

    }
}
