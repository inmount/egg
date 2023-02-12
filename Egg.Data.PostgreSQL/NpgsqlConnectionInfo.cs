using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data.PostgreSQL
{
    /// <summary>
    /// PostgreSQL数据库连接信息
    /// </summary>
    public class NpgsqlConnectionInfo : IDatabaseConnectionInfo
    {
        // 连接字符串
        private readonly string? _connectionString;

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public DatabaseTypes Type => DatabaseTypes.PostgreSQL;

        /// <summary>
        /// 获取数据库供应类
        /// </summary>
        public string ProviderName => "Egg.Data.PostgreSQL.NpgsqlProvider";

        /// <summary>
        /// PostgreSQL数据库连接信息
        /// </summary>
        public NpgsqlConnectionInfo()
        {

        }

        /// <summary>
        /// PostgreSQL数据库连接信息
        /// </summary>
        /// <param name="connectionString"></param>
        public NpgsqlConnectionInfo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ToConnectionString()
        {
            if (_connectionString is null) return "";
            return _connectionString;
        }
    }
}
