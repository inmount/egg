using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data
{
    /// <summary>
    /// 数据库供应器
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// 获取安全的值字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetValueString(string value);

        /// <summary>
        /// 获取名称定义字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        string GetNameString(string name);

        /// <summary>
        /// 获取函数定义
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetFuncString(string name, object[]? args);

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        IDatabaseConnectionBase GetDatabaseConnection(string connectionString);
    }
}
