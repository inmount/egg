using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.PostgreSQL
{
    /// <summary>
    /// 标准仓库接口
    /// </summary>
    public class SqlRepository : ISqlRepository
    {

        /// <summary>
        /// DB上下文
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="context"></param>
        public SqlRepository(DbContext context)
        {
            this.DbContext = context;
        }

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(string sql)
        {
            return ExcuteNonQueryAsync(sql).Result;
        }

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExcuteNonQueryAsync(string sql)
        {
            int res = 0;
            // 连接数据库
            System.Data.Common.DbConnection conn = this.DbContext.Database.GetDbConnection();
            using (var comm = conn.CreateCommand())
            {
                // 打开数据库连接
                conn.Open();
                // 生成sql语句
                comm.CommandText = sql;
                //Console.WriteLine(comm.CommandText);
                // 执行
                res = await comm.ExecuteNonQueryAsync();
                // 关闭连接
                conn.Close();
            }
            return res;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(string sql) where T : new()
        {
            return GetListAsync<T>(sql).Result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(string sql) where T : new()
        {
            // 新建列表
            List<T> result = new List<T>();
            // 反射类型
            Type tp = typeof(T);
            var pros = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // 连接数据库
            System.Data.Common.DbConnection conn = this.DbContext.Database.GetDbConnection();
            using (var comm = conn.CreateCommand())
            {
                // 打开数据库连接
                conn.Open();
                // 生成sql语句
                comm.CommandText = sql;
                // 执行
                var reader = await comm.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        T t = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i);
                            var pro = pros.Where(d => d.Name == fieldName).FirstOrDefault();
                            if (pro != null)
                            {
                                pro.SetValue(t, reader.GetValue(i));
                            }
                        }
                        result.Add(t);
                    }
                    reader.Close();
                }
                // 关闭连接
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T? GetOne<T>(string sql) where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T?> GetOneAsync<T>(string sql) where T : class, new()
        {
            // 新建列表
            T? result = null;
            // 反射类型
            Type tp = typeof(T);
            var pros = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // 连接数据库
            System.Data.Common.DbConnection conn = this.DbContext.Database.GetDbConnection();
            using (var comm = conn.CreateCommand())
            {
                // 打开数据库连接
                conn.Open();
                // 生成sql语句
                comm.CommandText = sql;
                // 执行
                var reader = await comm.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        T t = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i);
                            var pro = pros.Where(d => d.Name == fieldName).FirstOrDefault();
                            if (pro != null)
                            {
                                pro.SetValue(t, reader.GetValue(i));
                            }
                        }
                    }
                    reader.Close();
                }
                // 关闭连接
                conn.Close();
            }
            return result;
        }
    }
}
