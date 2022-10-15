using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// Egg专用数据库上下文
    /// </summary>
    public static class DbContextExtensions
    {

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="context"></param>
        public static bool EnsureCreated<T>(this DbContext context) where T : IDbCreater, new()
        {
            T creater = new T();
            return creater.EnsureCreated(context).Result;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetDbType(this DbContext context) => context.Database.ProviderName switch
        {
            var h when h == "Microsoft.EntityFrameworkCore.Sqlite" => "sqlite",
            _ => context.Database.ProviderName
        };
    }
}
