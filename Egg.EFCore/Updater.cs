using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.EFCore
{
    /// <summary>
    /// 更新器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Updater<T> where T : class
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
            var tp = typeof(T);
            var pros = tp.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            for (int i = 0; i < pros.Length; i++) this.Properties.Add(new UpdaterProperty(pros[i]));
        }
    }
}
