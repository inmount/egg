using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.CompilerServices;

namespace Egg.EFCore
{
    /// <summary>
    /// 更新器
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class Updater<TClass, TId> where TClass : IEntity<TId>
    {
        /// <summary>
        /// 定义属性集合
        /// </summary>
        public List<UpdaterProperty> Properties { get; }

        /// <summary>
        /// DB上下文
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// 更新器
        /// </summary>
        public Updater(DbContext context)
        {
            this.Context = context;
            this.Properties = new List<UpdaterProperty>();
            // 解析并添加所有的属性定义
            var tp = typeof(TClass);
            var pros = tp.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            for (int i = 0; i < pros.Length; i++) this.Properties.Add(new UpdaterProperty(pros[i]));
        }

        /// <summary>
        /// 添加更新字段
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Use(Expression<Func<TClass, object>> selector)
        {
            Console.WriteLine($"Body: {selector.Body}");
            return this;
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity)
        {
            return this;
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity, Expression<Func<TClass, bool>> predicate)
        {
            Console.WriteLine($"Body: {predicate.Body}");
            return this;
        }
    }
}
