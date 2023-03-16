using egg;
using Egg.Data.Entities;
using Egg.Data.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
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
        #region [=====属性=====]

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

        #endregion

        #region [=====工作单元=====]

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

        #endregion

        #region [=====函数=====]

        /// <summary>
        /// 获取受支持的标准数据库供应商
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDatabaseProvider GetDatabaseProvider(DatabaseTypes type)
        {
            string providerName = string.Empty;
            string providerDllPath = string.Empty;
            switch (type)
            {
                case DatabaseTypes.PostgreSQL:
                    providerName = "Egg.Data.PostgreSQL.NpgsqlProvider";
                    providerDllPath = "Egg.Data.PostgreSQL.dll";
                    break;
                case DatabaseTypes.Sqlite:
                case DatabaseTypes.Sqlite3:
                    providerName = "Egg.Data.Sqlite.SqliteProvider";
                    providerDllPath = "Egg.Data.Sqlite.dll";
                    break;
                default: throw new DatabaseException($"不支持的数据库类型\'{type.ToString()}\'");
            }
            Type? providerType = egg.Assembly.FindType(providerName, egg.IO.GetExecutionPath(providerDllPath));
            if (providerType is null) throw new DatabaseException($"未找到供应商\'{providerName}\'");
            return (IDatabaseProvider)Activator.CreateInstance(providerType);
        }

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
        /// 获取数据库命令
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommand GetCommand(string sql)
            => this.DatabaseConnectionBase.GetCommand(sql);

        // 获取连接配置值
        private string? GetConnectionValue(string key)
        {
            key = key.ToLower();
            string[] settings = this.ConnectionString.Split(';');
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                if (idx > 0)
                {
                    string name = setting.Substring(0, idx).Trim().ToLower();
                    if (key == name) return setting.Substring(idx + 1);
                }
            }
            return null;
        }

        // 获取连接用户名
        private string? GetConnectionUserName()
            => GetConnectionValue("username");

        #endregion

        #region [=====自动化=====]

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
                bool schemaExists = await AnyAsync(this.Provider.GetSchemaExistsSqlString(schemaName.ToNotNull()));
                if (!schemaExists)
                {
                    // 建立工作单元
                    using (var uow = BeginUnitOfWork())
                    {
                        // 执行表创建语句
                        await ExecuteNonQueryAsync(this.Provider.GetSchemaCreateSqlString(schemaName.ToNotNull()));
                        // 保存数据
                        await uow.CompleteAsync();
                    }
                }
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
                    if (!await AnyAsync(this.Provider.GetColumnExistsSqlString<T>(property)))
                        await ExecuteNonQueryAsync(Provider.GetColumnAddSqlString<T>(property));
                }
                // 保存数据
                await uow.CompleteAsync();
            }
            return true;
        }

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetDatabasesAsync()
            => await GetValuesAsync<string>(this.Provider.GetDatabasesQuerySqlString());

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetSchemasAsync(string dbName)
            => await GetValuesAsync<string>(this.Provider.GetSchemasQuerySqlString(dbName, GetConnectionUserName().ToNotNull()));

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetTablesAsync(string dbName, string? schema = null)
            => await GetValuesAsync<string>(this.Provider.GetTablesQuerySqlString(dbName, schema));

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetColumnsAsync(string dbName, string tabName)
            => await GetValuesAsync<string>(this.Provider.GetColumnsQuerySqlString(dbName, tabName));

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetColumnsAsync(string dbName, string schema, string tabName)
            => await GetValuesAsync<string>(this.Provider.GetColumnsQuerySqlString(dbName, schema, tabName));

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public List<string> GetDatabases()
            => GetValues<string>(this.Provider.GetDatabasesQuerySqlString());

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public List<string> GetSchemas(string dbName)
            => GetValues<string>(this.Provider.GetSchemasQuerySqlString(dbName, GetConnectionUserName().ToNotNull()));

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        public List<string> GetTables(string dbName, string? schema = null)
            => GetValues<string>(this.Provider.GetTablesQuerySqlString(dbName, schema));

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        public List<string> GetColumns(string dbName, string tabName)
            => GetValues<string>(this.Provider.GetColumnsQuerySqlString(dbName, tabName));

        /// <summary>
        /// 获取所有表字段
        /// </summary>
        /// <returns></returns>
        public List<string> GetColumns(string dbName, string schema, string tabName)
            => GetValues<string>(this.Provider.GetColumnsQuerySqlString(dbName, schema, tabName));

        #endregion

        #region [=====执行=====]

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

        #endregion

        #region [=====查询=====]

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int Read(string sql, Action<DbDataReader>? action = null)
            => this.DatabaseConnectionBase.Read(sql, reader =>
            {
                int res = 0;
                while (reader.Read())
                {
                    res++;
                    if (action != null) action(reader);
                }
                return res;
            });

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<int> ReadAsync(string sql, Action<DbDataReader>? action = null)
            => await this.DatabaseConnectionBase.ReadAsync(sql, reader =>
            {
                int res = 0;
                while (reader.Read())
                {
                    res++;
                    if (action != null) action(reader);
                }
                return res;
            });

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public int Read(string sql, Func<DbDataReader, int> func)
            => this.DatabaseConnectionBase.Read(sql, func);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task<int> ReadAsync(string sql, Func<DbDataReader, int> func)
            => this.DatabaseConnectionBase.ReadAsync(sql, func);

        /// <summary>
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Any(string sql)
            => Read(sql) > 0;

        /// <summary>
        /// 判断是否存在结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string sql)
            => await ReadAsync(sql) > 0;

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetValue<T>(string sql)
        {
            T res = default(T);
            bool found = false;
            Read(sql, reader =>
            {
                if (found) return;
                res = reader.ToValue<T>();
                found = true;
            });
            if (res is null) throw new DatabaseException("获取结果失败");
            return res;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T> GetValueAsync<T>(string sql)
        {
            T res = default(T);
            bool found = false;
            await ReadAsync(sql, reader =>
             {
                 if (found) return;
                 res = reader.ToValue<T>();
                 found = true;
             });
            if (res is null) throw new DatabaseException("获取结果失败");
            return res;
        }

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetValues<T>(string sql)
        {
            List<T> res = new List<T>();
            Read(sql, reader =>
            {
                res.Add(reader.ToValue<T>());
            });
            return res;
        }

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetValuesAsync<T>(string sql)
        {
            List<T> res = new List<T>();
            await ReadAsync(sql, reader =>
             {
                 res.Add(reader.ToValue<T>());
             });
            return res;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T? GetRow<T>(string sql) where T : class
        {
            var mapper = new EntityMapper<T>();
            T? res = default(T);
            bool found = false;
            Read(sql, reader =>
            {
                if (found) return;
                res = reader.ToEntity(mapper);
                found = true;
            });
            return res;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T?> GetRowAsync<T>(string sql) where T : class
        {
            var mapper = new EntityMapper<T>();
            T? res = default(T);
            bool found = false;
            await ReadAsync(sql, reader =>
             {
                 if (found) return;
                 res = reader.ToEntity(mapper);
                 found = true;
             });
            return res;
        }

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetRows<T>(string sql) where T : class
        {
            var mapper = new EntityMapper<T>();
            List<T> res = new List<T>();
            Read(sql, reader =>
            {
                res.Add(reader.ToEntity(mapper));
            });
            return res;
        }


        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetRowsAsync<T>(string sql) where T : class
        {
            var mapper = new EntityMapper<T>();
            List<T> res = new List<T>();
            await ReadAsync(sql, reader =>
             {
                 res.Add(reader.ToEntity(mapper));
             });
            return res;
        }

        #endregion

        #region [=====构造函数=====]

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

        #endregion
    }
}
