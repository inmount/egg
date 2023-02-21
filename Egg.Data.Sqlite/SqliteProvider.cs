using Egg.Data.Extensions;
using Egg.Data.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        {
            return new SqliteConnectionBase(connectionString);
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
        {
            return $"\"{name}\"";
        }

        /// <summary>
        /// 获取值字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetValueString(string value)
        {
            return $"'{value.Replace("'", "''")}'";
        }

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
                    return $"{GetNameString(column.GetColumnAttributeName())} {column.GetColumnAttributeType()} NOT NULL CONSTRAINT \"PK_{tableName}\" PRIMARY KEK AUTOINCREMENT";
                }
                else
                {
                    return $"{GetNameString(column.GetColumnAttributeName())} {column.GetColumnAttributeType()} NOT NULL CONSTRAINT \"PK_{tableName}\" PRIMARY KEY";
                }
            }
            else
            {
                return $"{GetNameString(column.GetColumnAttributeName())} {column.GetColumnAttributeType()} {(column.IsNullable() ? "NULL" : "NOT NULL")}";
            }
        }

        /// <summary>
        /// 获取确保数据库创建语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetTableCreateSqlString<T>(T entity)
        {
            // 申明拼接字符串
            StringBuilder sb = new StringBuilder();
            // 创建表
            Type type = typeof(T);
            string tableName = type.Name;
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                if (!tableAttr.Name.IsNullOrWhiteSpace()) tableName = tableAttr.Name;
            }
            // 拼接语句
            sb.AppendLine($"CREATE TABLE IF NOT EXISTS {GetNameString(tableName)}(");
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

        /// <summary>
        /// 确保数据库创建
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> TableCreated<T>(DatabaseConnection conn, T entity)
        {
            // 创建表
            Type type = typeof(T);
            string tableName = type.Name;
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            string createTableSql = GetTableCreateSqlString(entity);
            using (var uow = conn.BeginUnitOfWork())
            {
                // 执行表创建语句
                await conn.ExecuteNonQueryAsync(createTableSql);
            }
            //// 获取所有字段
            //foreach (IMutableProperty property in entity.GetProperties())
            //{
            //    string columnName = property.GetColumnBaseName();
            //    string checkFieldSql = $"select * from [sqlite_master] where type='table' and name ='{tableName}' and (sql like '%[{columnName}]%' or sql like '%\"{columnName}\"%' or sql like '%,{columnName},%'or sql like '%({columnName},%'or sql like '%,{columnName})%' or sql like '% {columnName} %');";
            //    comm.CommandText = checkFieldSql;
            //    var reader = await comm.ExecuteReaderAsync();
            //    if (reader?.Read() ?? false)
            //    {
            //        string columnSql = GetColumnSql(entity, property);
            //        string dataSql = (string)reader["sql"];
            //        reader.Close();
            //        // 当列信息不匹配时，执行更新
            //        if (dataSql.IndexOf(columnSql) < 0)
            //        {
            //            comm.CommandText = GetAlterColumnSql(entity, property);
            //            Console.WriteLine(comm.CommandText);
            //            await comm.ExecuteNonQueryAsync();
            //        }
            //    }
            //    else
            //    {
            //        reader?.Close();
            //        comm.CommandText = GetAddColumnSql(entity, property);
            //        Console.WriteLine(comm.CommandText);
            //        await comm.ExecuteNonQueryAsync();
            //    }

            //}
            return true;
        }
    }
}
