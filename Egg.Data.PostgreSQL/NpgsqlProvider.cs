using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Egg.Data.PostgreSQL
{
    /// <summary>
    /// PostgreSQL数据库语法供应器
    /// </summary>
    public class NpgsqlProvider : IDatabaseProvider
    {
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

        public string GetColumnAddSqlString(string tableName, PropertyInfo column)
        {
            throw new NotImplementedException();
        }

        public string GetColumnExistsSqlString(string tableName, PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库基础连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDatabaseConnectionBase GetDatabaseConnection(string connectionString) 
            => new NpgsqlConnectionBase(connectionString);

        public string GetFullTableName<T>()
        {
            throw new NotImplementedException();
        }

        public string GetFuncString(string name, object[]? args)
        {
            throw new NotImplementedException();
        }

        public string GetTableCreateSqlString<T>()
        {
            throw new NotImplementedException();
        }

        public string GetTableExistsSqlString<T>()
        {
            throw new NotImplementedException();
        }

    }
}
