using Egg.EFCore.Dbsets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// Egg专用数据库上下文
    /// </summary>
    public static class DbContextExtensions
    {
        // 数据表对象名称
        private const string TYPE_NAME_DBSET = "Microsoft.EntityFrameworkCore.DbSet`1";
        private const string TYPE_NAME_ENTITY = "Egg.EFCore.Dbsets.Entity`1";

        // 获取实例基类定义类型
        private static Type? GetEntityType(Type tp)
        {
            string tpName = tp.Namespace + "." + tp.Name;
            if (tpName == TYPE_NAME_ENTITY) return tp;
            if (tp.BaseType is null) return null;
            return GetEntityType(tp.BaseType);
        }

        /// <summary>
        /// 创建数据仓库
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object CreateRepositoty(DbContext context, Type entity, Type key)
        {
            Type tp = typeof(Repository<,>);
            //指定泛型的具体类型
            Type tpNew = tp.MakeGenericType(new Type[] { entity, key });
            //创建一个list返回
            return Activator.CreateInstance(tpNew, new object[] { context });
        }

        /// <summary>
        /// 生成数据仓库集合
        /// </summary>
        /// <param name="context"></param>
        public static List<object> GetRepositories(this DbContext context)
        {
            List<object> list = new List<object>();
            var pros = context.GetType().GetProperties();
            foreach (var pro in pros)
            {
                var tp = pro.PropertyType;
                string tpName = tp.Namespace + "." + tp.Name;
                if (tpName != TYPE_NAME_DBSET) continue;
                var tpEntity = tp.GenericTypeArguments[0];
                var tpEntityType = GetEntityType(tpEntity);
                if (tpEntityType is null) continue;
                var tpEntityKeyType = tpEntityType.GenericTypeArguments[0];
                var obj = CreateRepositoty(context, tpEntity, tpEntityKeyType);
                list.Add(obj);
            }
            return list;
        }

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
