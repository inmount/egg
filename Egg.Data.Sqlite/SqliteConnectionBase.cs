using Egg.Data.Entities;
using System;
using System.Collections.Generic;
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
        private Microsoft.Data.Sqlite.SqliteConnection? _dbc = null;
        // 连接字符串
        private readonly string _connectionString;

        /// <summary>
        /// Sqlite原始连接
        /// </summary>
        public Microsoft.Data.Sqlite.SqliteConnection? SqliteConnection { get { return _dbc; } }

        /// <summary>
        /// Sqlite基础连接
        /// </summary>
        /// <param name="connectionString"></param>
        public SqliteConnectionBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (_dbc is null) return;
            try { _dbc.Close(); } catch { };
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetRow<T>(string sql)
        {
            T res;
            var mapper = new EntityMapper<T>();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {

                    if (reader.Read())
                    {
                        res = mapper.Map(pro =>
                        {
                            int idx = reader.GetOrdinal(pro.Name);
                            if (idx >= 0) return reader.GetValue(idx);
                            return null;
                        });
                    }
                    else
                    {
                        res = (T)Activator.CreateInstance(typeof(T));
                    }

                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetRows<T>(string sql)
        {
            List<T> res = new List<T>();
            var mapper = new EntityMapper<T>();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        var item = mapper.Map(pro =>
                        {
                            int idx = reader.GetOrdinal(pro.Name);
                            if (idx >= 0) return reader.GetValue(idx);
                            return null;
                        });
                        res.Add(item);
                    }
                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <exception cref="DatabaseException"></exception>
        public void Open()
        {
            if (_dbc != null) throw new DatabaseException($"数据库已存在连接");
            _dbc = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString);

            _dbc.Open();
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
        /// 执行sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                return await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T> GetRowAsync<T>(string sql)
        {
            T res;
            var mapper = new EntityMapper<T>();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {

                    if (reader.Read())
                    {
                        res = mapper.Map(pro =>
                        {
                            int idx = reader.GetOrdinal(pro.Name);
                            if (idx >= 0) return reader.GetValue(idx);
                            return null;
                        });
                    }
                    else
                    {
                        res = (T)Activator.CreateInstance(typeof(T));
                    }

                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> GetRowsAsync<T>(string sql)
        {
            List<T> res = new List<T>();
            var mapper = new EntityMapper<T>();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, _dbc))
            {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        var item = mapper.Map(pro =>
                        {
                            int idx = reader.GetOrdinal(pro.Name);
                            if (idx >= 0) return reader.GetValue(idx);
                            return null;
                        });
                        res.Add(item);
                    }
                    reader.Close();
                }
            }
            return res;
        }
    }
}
