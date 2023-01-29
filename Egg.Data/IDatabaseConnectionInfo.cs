using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public interface IDatabaseConnectionInfo
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DatabaseTypes Type { get; }

        /// <summary>
        /// 供应类名称
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// 转化为连接字符串
        /// </summary>
        /// <returns></returns>
        string ToConnectionString();
    }
}
