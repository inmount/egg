using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
