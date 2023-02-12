using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Egg.Data.Extensions
{
    /// <summary>
    /// 属性扩展
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 是否含有Key特性
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static bool IsKey(this PropertyInfo pro)
        {
            return pro.GetCustomAttributes<KeyAttribute>().Any();
        }

        /// <summary>
        /// 是否含有AutoIncrement特性
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static bool IsAutoIncrement(this PropertyInfo pro)
        {
            return pro.GetCustomAttributes<AutoIncrementAttribute>().Any();
        }
    }
}
