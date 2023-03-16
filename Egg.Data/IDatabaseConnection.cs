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
    public interface IDatabaseConnection : IDatabaseConnectionBase
    {
        /// <summary>
        /// 数据库供应商
        /// </summary>
        IDatabaseProvider Provider { get; }

        #region [=====数据库基础=====]

        /// <summary>
        /// 数据库基础连接
        /// </summary>
        IDatabaseConnectionBase DatabaseConnectionBase { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        string ConnectionString { get; }

        #endregion

        #region [=====工作单元=====]

        /// <summary>
        /// 事务单元
        /// </summary>
        UnitOfWork? UnitOfWork { get; }

        /// <summary>
        /// 开始一个新的事务单元
        /// </summary>
        /// <returns></returns>
        UnitOfWork BeginUnitOfWork();

        /// <summary>
        /// 结束当前事务单元
        /// </summary>
        void EndUnitOfWork();

        #endregion

        #region [=====读取数据=====]

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

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        int Read(string sql, Action<DbDataReader>? action = null);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<int> ReadAsync(string sql, Action<DbDataReader>? action = null);

        #endregion

        #region [=====自动化=====]

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetDatabasesAsync();

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetSchemasAsync(string dbName);

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetTablesAsync(string dbName, string? schema = null);

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetColumnsAsync(string dbName, string tabName);

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetColumnsAsync(string dbName, string schema, string tabName);

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        List<string> GetDatabases();

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        List<string> GetSchemas(string dbName);

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        List<string> GetTables(string dbName, string? schema = null);

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        List<string> GetColumns(string dbName, string tabName);

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        List<string> GetColumns(string dbName, string schema, string tabName);

        #endregion

    }
}
