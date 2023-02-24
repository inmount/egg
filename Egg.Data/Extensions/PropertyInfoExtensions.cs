using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        /// 是否为可空字段
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static bool IsNullable(this PropertyInfo pro)
        {
            if (pro.PropertyType.IsNullable()) return true;
            return pro.GetCustomAttributes<RequiredAttribute>().Any();
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

        /// <summary>
        /// 获取列属性名称
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo pro)
        {
            var columnAttr = pro.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr is null) return pro.Name;
            if (columnAttr.Name.IsNullOrWhiteSpace()) return pro.Name;
            return columnAttr.Name ?? string.Empty;
        }
    }
}
