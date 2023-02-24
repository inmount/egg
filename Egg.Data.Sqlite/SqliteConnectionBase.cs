using Egg.Data.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }

        #region [=====读取数据=====]

        // 读取数据
        private TClass? ReaderToEntity<TClass>(EntityMapper<TClass> mapper, SqliteDataReader reader) where TClass : class
        {
            return mapper.Map(pro =>
            {
                int idx = reader.GetOrdinal(pro.Name);
                if (idx >= 0) return reader.GetValue(idx);
                return null;
            });
        }

        // 读取数据
        private T? ReadToEntity<T>(EntityMapper<T> mapper, SqliteDataReader reader) where T : class
        {
            if (reader.Read()) return ReaderToEntity(mapper, reader);
            return default(T);
        }

        // 读取数据
        private List<T> ReadToList<T>(EntityMapper<T> mapper, SqliteDataReader reader) where T : class
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                var item = ReaderToEntity(mapper, reader);
                if (item != null) list.Add(item);
            }
            return list;
        }

        // 读取数据
        private T ReadToValue<T>(SqliteDataReader reader)
        {
            if (reader.Read()) return reader[0].ConvertTo<T>();
            throw new DatabaseException("读取失败");
        }

        // 读取数据
        private List<T> ReadToValues<T>(SqliteDataReader reader)
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                var item = reader[0].ConvertTo<T>();
                if (item != null) list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetValue<T>(string sql)
        {
            T res;
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    res = ReadToValue<T>(reader);
                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T> GetValueAsync<T>(string sql)
        {
            T res;
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    res = ReadToValue<T>(reader);
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
        public List<T> GetValues<T>(string sql)
        {
            List<T> res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    res = ReadToValues<T>(reader);
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
        public async Task<List<T>> GetValuesAsync<T>(string sql)
        {
            List<T> res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    res = ReadToValues<T>(reader);
                    reader.Close();
                }
            }
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
            T? res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    res = ReadToEntity(mapper, reader);
                    reader.Close();
                }
            }
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
            T? res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {

                    res = ReadToEntity(mapper, reader);
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
        public List<T> GetRows<T>(string sql) where T : class
        {
            List<T> res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    res = ReadToList(mapper, reader);
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
        public async Task<List<T>> GetRowsAsync<T>(string sql) where T : class
        {
            List<T> res;
            var mapper = new EntityMapper<T>();
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    res = ReadToList(mapper, reader);
                    reader.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 判断是否有返回数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Any(string sql)
        {
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// 判断是否有返回数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string sql)
        {
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                using (SqliteDataReader reader = await sqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior.Default))
                {
                    return reader.Read();
                }
            }
        }

        #endregion

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <exception cref="DatabaseException"></exception>
        public void Open()
        {
            if (_dbc != null) throw new DatabaseException($"数据库已存在连接");
            _dbc = new SqliteConnection(_connectionString);

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
            using (SqliteCommand sqlCommand = new SqliteCommand(sql, _dbc))
            {
                return await sqlCommand.ExecuteNonQueryAsync();
            }
        }

    }
}
