using egg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DatabaseConnection : IDatabaseConnectionBase
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
        {
            this.UnitOfWork?.Dispose();
        }

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
                    return (IDatabaseProvider)Activator.CreateInstance(Type.GetType("Egg.Data.PostgreSQL.NpgsqlProvider"));
                case DatabaseTypes.Sqlite:
                case DatabaseTypes.Sqlite3:
                    return (IDatabaseProvider)Activator.CreateInstance(Type.GetType("Egg.Data.Sqlite.SqliteProvider"));
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
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetRow<T>(string sql)
        {
            return this.DatabaseConnectionBase.GetRow<T>(sql);
        }

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetRows<T>(string sql)
        {
            return this.DatabaseConnectionBase.GetRows<T>(sql);
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public void Open()
        {
            this.DatabaseConnectionBase.Open();
        }

        /// <summary>
        /// 断开数据库
        /// </summary>
        public void Close()
        {
            try
            {
                this.DatabaseConnectionBase.Close();
            }
            catch { }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T> GetRowAsync<T>(string sql)
        {
            return await this.DatabaseConnectionBase.GetRowAsync<T>(sql);
        }

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<List<T>> GetRowsAsync<T>(string sql)
        {
            return this.DatabaseConnectionBase.GetRowsAsync<T>(sql);
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

    }
}
