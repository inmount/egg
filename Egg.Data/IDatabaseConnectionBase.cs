using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public interface IDatabaseConnectionBase : IDisposable
    {

        /// <summary>
        /// 是否连接
        /// </summary>
        bool IsOpened { get; }

        #region [=====数据库基础=====]

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库
        /// </summary>
        void Close();

        /// <summary>
        /// 获取数据库命令
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DbCommand GetCommand(string sql);

        #endregion

        #region [=====执行=====]

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteNonQueryAsync(string sql);

        #endregion

        #region [=====读取=====]

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        int Read(string sql, Func<DbDataReader, int> func);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<int> ReadAsync(string sql, Func<DbDataReader, int> func);

        #endregion

    }
}
