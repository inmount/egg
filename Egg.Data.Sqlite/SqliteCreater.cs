using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using Egg;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;
using System.Security.Cryptography;
using Egg.EFCore.Sqlite;
using Egg.Data.Sqlite;
using Egg.EFCore.Extensions;

namespace Egg.EFCore
{
    /// <summary>
    /// Sqlite创建器
    /// </summary>
    public class SqliteCreater : IDbCreater
    {
        // 数据库供应商
        private readonly SqliteProvider _provider;

        /// <summary>
        /// Sqlite创建器
        /// </summary>
        public SqliteCreater()
        {
            _provider = new SqliteProvider();
        }

        /// <summary>
        /// 确保数据库创建
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> EnsureCreated(DbContext context)
        {
            // 获取数据库连接
            System.Data.Common.DbConnection conn = context.Database.GetDbConnection();
            {
                using (var comm = conn.CreateCommand())
                {
                    try
                    {
                        // 打开数据库连接
                        conn.Open();
                        // 获取根类型
                        var tp = context.GetType();
                        var entities = context.Model.GetEntityTypes();
                        foreach (var entity in entities)
                        {
                            // 创建表
                            string tableName = entity.GetTableName();
                            string createTableSql = GetCreateTableSql(entity);
                            comm.CommandText = createTableSql;
                            await comm.ExecuteNonQueryAsync();
                            // 获取所有字段
                            foreach (IMutableProperty property in entity.GetProperties())
                            {
                                string columnName = property.GetColumnBaseName();
                                string checkFieldSql = $"select * from [sqlite_master] where type='table' and name ='{tableName}' and (sql like '%[{columnName}]%' or sql like '%\"{columnName}\"%' or sql like '%,{columnName},%'or sql like '%({columnName},%'or sql like '%,{columnName})%' or sql like '% {columnName} %');";
                                comm.CommandText = checkFieldSql;
                                var reader = await comm.ExecuteReaderAsync();
                                if (reader?.Read() ?? false)
                                {
                                    string columnSql = GetColumnSql(entity, property);
                                    string dataSql = (string)reader["sql"];
                                    reader.Close();
                                    // 当列信息不匹配时，执行更新
                                    if (dataSql.IndexOf(columnSql) < 0)
                                    {
                                        comm.CommandText = GetAlterColumnSql(entity, property);
                                        Console.WriteLine(comm.CommandText);
                                        await comm.ExecuteNonQueryAsync();
                                    }
                                }
                                else
                                {
                                    reader?.Close();
                                    comm.CommandText = GetAddColumnSql(entity, property);
                                    Console.WriteLine(comm.CommandText);
                                    await comm.ExecuteNonQueryAsync();
                                }

                            }
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取数据列创建语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetAddColumnSql(IEntityType table, IMutableProperty column)
        {
            return $"ALTER TABLE {_provider.GetNameString(table.GetTableName())} ADD COLUMN {GetColumnSql(table, column)};";
        }

        /// <summary>
        /// 获取数据列修改语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetAlterColumnSql(IEntityType table, IMutableProperty column)
        {
            return $"ALTER TABLE {_provider.GetNameString(table.GetTableName())} ALTER COLUMN {GetColumnSql(table, column)};";
        }

        /// <summary>
        /// 获取数据列创建语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetColumnSql(IEntityType table, IMutableProperty column)
        {
            if (column.IsPrimaryKey())
            {
                if (column.IsAutoIncrement())
                {
                    return $"{_provider.GetNameString(column.GetColumnBaseName())} {column.GetColumnType()} NOT NULL CONSTRAINT \"PK_{table.GetTableName()}\" PRIMARY KEK AUTOINCREMENT";
                }
                else
                {
                    return $"{_provider.GetNameString(column.GetColumnBaseName())} {column.GetColumnType()} NOT NULL CONSTRAINT \"PK_{table.GetTableName()}\" PRIMARY KEY";
                }
            }
            else
            {
                return $"{_provider.GetNameString(column.GetColumnBaseName())} {column.GetColumnType()} {(column.IsColumnNullable() ? "NULL" : "NOT NULL")}";
            }
        }

        /// <summary>
        /// 获取创建表的语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string GetCreateTableSql(IEntityType table)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = table.GetTableName();
            bool isFirst = true;
            // 拼接语句
            sb.Append($"CREATE TABLE IF NOT EXISTS {_provider.GetNameString(tableName)}(\n");
            foreach (IMutableProperty property in table.GetProperties())
            {
                if (isFirst) { isFirst = false; } else { sb.Append(','); sb.AppendLine(); }
                sb.Append("    ");
                sb.Append(GetColumnSql(table, property));
            }
            // 拼接语句
            sb.AppendLine();
            sb.Append($");");
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// 获取Sql语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>        
        public string GetEnsureCreatedSql(DbContext context)
        {
            StringBuilder sb = new StringBuilder();
            // 获取根类型
            var tp = context.GetType();
            var entities = context.Model.GetEntityTypes();
            foreach (var entity in entities)
            {
                // 创建表
                string tableName = entity.GetTableName();
                sb.Append(GetCreateTableSql(entity));
                // 获取所有字段
                foreach (IMutableProperty property in entity.GetProperties())
                {
                    sb.Append(GetAddColumnSql(entity, property));
                    sb.AppendLine();
                    sb.Append(GetAlterColumnSql(entity, property));
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
