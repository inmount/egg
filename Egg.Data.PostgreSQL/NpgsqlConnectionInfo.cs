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
        private readonly string _connectionString;

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
        public string ToConnectionString() => _connectionString;

        /// <summary>
        /// 创建PostgreSQL数据库连接信息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseException"></exception>
        public static NpgsqlConnectionInfo Create(string server, string userName, string password, string dbName)
        {
            // 动态拼接连接字符串
            StringBuilder sb = new StringBuilder();
            sb.Append($"server={server};");
            sb.Append($"username={userName};");
            sb.Append($"password={password};");
            sb.Append($"database={dbName};");
            return new NpgsqlConnectionInfo(sb.ToString());
        }

        /// <summary>
        /// 创建PostgreSQL数据库连接信息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static NpgsqlConnectionInfo Create(string server, int port, string userName, string password, string dbName)
        {
            // 动态拼接连接字符串
            StringBuilder sb = new StringBuilder();
            sb.Append($"server={server};");
            sb.Append($"port={Convert.ToString(port)};");
            sb.Append($"username={userName};");
            sb.Append($"password={password};");
            sb.Append($"database={dbName};");
            return new NpgsqlConnectionInfo(sb.ToString());
        }
    }
}
