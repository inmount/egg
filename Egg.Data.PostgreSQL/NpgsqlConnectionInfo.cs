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
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public DatabaseTypes Type => DatabaseTypes.PostgreSQL;

        /// <summary>
        /// 获取数据库供应类
        /// </summary>
        public string ProviderName => "Egg.Data.PostgreSQL.NpgsqlProvider";

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ToConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
