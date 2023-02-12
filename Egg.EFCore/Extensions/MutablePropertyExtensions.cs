using Egg.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Egg.EFCore.Extensions
{
    /// <summary>
    /// 属性扩展
    /// </summary>
    public static class MutablePropertyExtensions
    {
        /// <summary>
        /// 是否拥有自动增长特性
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static bool IsAutoIncrement(this IMutableProperty pro)
        {
            return pro.PropertyInfo.GetCustomAttributes<AutoIncrementAttribute>().Any();
        }
    }
}
