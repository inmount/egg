using Egg.Data.Entities;
using Egg.Data.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data.Sqlite
{
    /// <summary>
    /// Sqlite基础连接
    /// </summary>
    public class SqliteConnectionBase : IDatabaseConnectionBase
    {
        //数据库的连接管理器
        private SqliteConnection? _dbc = null;
        // 连接字符串
        private readonly string _connectionString;

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        /// Sqlite原始连接
        /// </summary>
        public SqliteConnection? SqliteConnection { get { return _dbc; } }

        /// <summary>
        /// Sqlite基础连接
        /// </summary>
        /// <param name="connectionString"></param>
        public SqliteConnectionBase(string connectionString)
        {
            _connectionString = connectionString;
            this.IsOpened = false;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (_dbc is null) return;
            try { _dbc.Close(); } catch { };
            this.IsOpened = false;
        }

        /// <summary>
        /// 获取数据库命令
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommand GetCommand(string sql)
            => new SqliteCommand(sql, _dbc);

        #region [=====脚本执行=====]

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            using (var sqlCommand = GetCommand(sql))
                return sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            using (var sqlCommand = GetCommand(sql))
                return await sqlCommand.ExecuteNonQueryAsync();
        }

        #endregion

        #region [=====读取数据=====]

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public int Read(string sql, Func<DbDataReader, int> func)
        {
            int res = 0;
            using (DbCommand sqlCommand = GetCommand(sql))
            {
                using (DbDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    res = func(reader);
                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<int> ReadAsync(string sql, Func<DbDataReader, int> func)
        {
            int res = 0;
            using (DbCommand sqlCommand = GetCommand(sql))
            {
                using (DbDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    res = func(reader);
                    reader.Close();
                }
            }
            return res;
        }

        #endregion

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <exception cref="DatabaseException"></exception>
        public void Open()
        {
            if (this.IsOpened) throw new DatabaseException($"数据库已存在连接");
            if (_dbc != null) throw new DatabaseException($"数据库已存在连接");
            _dbc = new SqliteConnection(_connectionString);
            _dbc.Open();
            this.IsOpened = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

    }
}
