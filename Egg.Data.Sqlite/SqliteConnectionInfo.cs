using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data.Sqlite
{
    /// <summary>
    /// Sqlite连接信息
    /// </summary>
    public class SqliteConnectionInfo : IDatabaseConnectionInfo
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
        public string ProviderName => "Egg.Data.Sqlite.SqliteProvider";

        /// <summary>
        /// 存储路径
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// PostgreSQL数据库连接信息
        /// </summary>
        public SqliteConnectionInfo()
        {
            _connectionString = null;
        }

        /// <summary>
        /// PostgreSQL数据库连接信息
        /// </summary>
        /// <param name="connectionString"></param>
        public SqliteConnectionInfo(string connectionString)
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
            if (_connectionString is null)
            {
                // 动态拼接连接字符串
                StringBuilder sb = new StringBuilder();
                // 设置存储路径
                if (this.Path.IsEmpty()) throw new DatabaseException($"缺少必要的存储路径配置");
                sb.Append($"Data Source={this.Path};");
                if (!this.Password.IsEmpty())
                    sb.Append($"Password={this.Password};");
                return sb.ToString();
            }
            return _connectionString;
        }
    }
}
