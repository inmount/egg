using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egg.EFCore.Sqlite
{
    /// <summary>
    /// DbContext扩展
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="context"></param>
        public static bool EnsureCreatedSqlite(this DbContext context)
        {
            return context.EnsureCreated<SqliteCreater>();
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(this DbContext context, string sql)
        {
            int res = 0;
            // 连接数据库
            System.Data.Common.DbConnection conn = context.Database.GetDbConnection();
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
        /// 执行SQL语句
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this DbContext context, string sql)
        {
            return context.ExecuteNonQueryAsync(sql).Result;
        }
    }
}
