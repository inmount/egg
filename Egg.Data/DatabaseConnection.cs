using egg;
using Egg.Data.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        /// <summary>
        /// 数据库供应商
        /// </summary>
        public IDatabaseProvider Provider { get; }

        /// <summary>
        /// 数据库基础连接
        /// </summary>
        public IDatabaseConnectionBase DatabaseConnectionBase { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 事务单元
        /// </summary>
        public UnitOfWork? UnitOfWork { get; internal set; }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsOpened => this.DatabaseConnectionBase.IsOpened;

        /// <summary>
        /// 开始一个新的事务单元
        /// </summary>
        /// <returns></returns>
        public UnitOfWork BeginUnitOfWork()
        {
            UnitOfWork uow = new UnitOfWork(this);
            this.UnitOfWork = uow;
            return uow;
        }

        /// <summary>
        /// 结束当前事务单元
        /// </summary>
        public void EndUnitOfWork()
            => this.UnitOfWork?.Dispose();

        /// <summary>
        /// 获取受支持的标准数据库供应商
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDatabaseProvider GetDatabaseProvider(DatabaseTypes type)
        {
            switch (type)
            {
                case DatabaseTypes.PostgreSQL:
                    return (IDatabaseProvider)Activator.CreateInstance(egg.Assembly.FindType("Egg.Data.PostgreSQL.NpgsqlProvider"));
                case DatabaseTypes.Sqlite:
                case DatabaseTypes.Sqlite3:
                    return (IDatabaseProvider)Activator.CreateInstance(egg.Assembly.FindType("Egg.Data.Sqlite.SqliteProvider"));
                default: throw new DatabaseException($"不支持的数据库类型\'{type.ToString()}\'");
            }
        }

        /// <summary>
        /// 执行Sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            // 当前如果没有事务单元，则直接执行，有事务单元，则将代码添加到事务中
            if (this.UnitOfWork is null) return this.DatabaseConnectionBase.ExecuteNonQuery(sql);
            this.UnitOfWork.Add(sql);
            return 0;
        }

        /// <summary>
        /// 执行Sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            // 当前如果没有事务单元，则直接执行，有事务单元，则将代码添加到事务中
            if (this.UnitOfWork is null) return await this.DatabaseConnectionBase.ExecuteNonQueryAsync(sql);
            this.UnitOfWork.Add(sql);
            return 0;
        }

        /// <summary>
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Any(string sql)
            => this.DatabaseConnectionBase.Any(sql);

        /// <summary>
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string sql)
            => await this.DatabaseConnectionBase.AnyAsync(sql);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetValue<T>(string sql)
            => this.DatabaseConnectionBase.GetValue<T>(sql);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<T> GetValueAsync<T>(string sql)
            => this.DatabaseConnectionBase.GetValueAsync<T>(sql);

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetValues<T>(string sql)
            => this.DatabaseConnectionBase.GetValues<T>(sql);

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<List<T>> GetValuesAsync<T>(string sql)
            => this.DatabaseConnectionBase.GetValuesAsync<T>(sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T? GetRow<T>(string sql) where T : class
            => this.DatabaseConnectionBase.GetRow<T>(sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T?> GetRowAsync<T>(string sql) where T : class
            => await this.DatabaseConnectionBase.GetRowAsync<T>(sql);

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetRows<T>(string sql) where T : class
            => this.DatabaseConnectionBase.GetRows<T>(sql);


        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<List<T>> GetRowsAsync<T>(string sql) where T : class
            => this.DatabaseConnectionBase.GetRowsAsync<T>(sql);

        /// <summary>
        /// 连接数据库
        /// </summary>
        public void Open()
            => this.DatabaseConnectionBase.Open();

        /// <summary>
        /// 断开数据库
        /// </summary>
        public void Close()
            => this.DatabaseConnectionBase.Close();

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try { this.Close(); } catch { }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 数据库链接呢
        /// </summary>
        /// <param name="type"></param>
        /// <param name="connectionString"></param>
        public DatabaseConnection(DatabaseTypes type, string connectionString)
        {
            // 设置数据库供应商
            Provider = GetDatabaseProvider(type);
            // 设置连接字符串
            ConnectionString = connectionString;
            // 设置基础连接
            DatabaseConnectionBase = Provider.GetDatabaseConnection(ConnectionString);
        }

        /// <summary>
        /// 数据库链接呢
        /// </summary>
        /// <param name="info"></param>
        public DatabaseConnection(IDatabaseConnectionInfo info)
        {
            // 设置数据库供应商
            var type = egg.Assembly.FindType(info.ProviderName);
            Provider = (IDatabaseProvider)Activator.CreateInstance(type);
            // 设置连接字符串
            ConnectionString = info.ToConnectionString();
            // 设置基础连接
            DatabaseConnectionBase = Provider.GetDatabaseConnection(ConnectionString);
        }

        /// <summary>
        /// 确保数据库创建
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> TableCreated<T>()
        {
            Type type = typeof(T);
            // 处理架构
            string? schemaName = type.GetSchemaName();
            if (!schemaName.IsNullOrWhiteSpace())
            {

            }
            // 创建表
            string tableName = type.GetTableName();
            // 判断表是否存在，不存在则执行表创建
            bool tableExists = false;
            try { tableExists = await AnyAsync(this.Provider.GetTableExistsSqlString<T>()); } catch { }
            if (!tableExists)
            {
                // 建立工作单元
                using (var uow = BeginUnitOfWork())
                {
                    // 执行表创建语句
                    await ExecuteNonQueryAsync(this.Provider.GetTableCreateSqlString<T>());
                    // 保存数据
                    await uow.CompleteAsync();
                }
            }
            // 建立工作单元
            using (var uow = BeginUnitOfWork())
            {
                // 获取所有字段
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    // 判断字段是否存在，不存在则添加
                    if (!await AnyAsync(this.Provider.GetColumnExistsSqlString(tableName, property)))
                        await ExecuteNonQueryAsync(Provider.GetColumnAddSqlString(tableName, property));
                }
                // 保存数据
                await uow.CompleteAsync();
            }
            return true;
        }
    }
}
