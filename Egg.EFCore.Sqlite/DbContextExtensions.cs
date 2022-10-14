using Microsoft.EntityFrameworkCore;
using System;

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
    }
}
