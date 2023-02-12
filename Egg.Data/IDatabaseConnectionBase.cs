using System;
using System.Collections.Generic;
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
        /// 打开数据库连接
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库
        /// </summary>
        void Close();

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T GetRow<T>(string sql);

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        List<T> GetRows<T>(string sql);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteNonQueryAsync(string sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<T> GetRowAsync<T>(string sql);

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> GetRowsAsync<T>(string sql);
    }
}
