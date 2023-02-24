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
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool Any(string sql);

        /// <summary>
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(string sql);

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T GetValue<T>(string sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<T> GetValueAsync<T>(string sql);

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        List<T> GetValues<T>(string sql);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> GetValuesAsync<T>(string sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T? GetRow<T>(string sql) where T : class;

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<T?> GetRowAsync<T>(string sql) where T : class;

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        List<T> GetRows<T>(string sql) where T : class;

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> GetRowsAsync<T>(string sql) where T : class;

        #endregion

    }
}
