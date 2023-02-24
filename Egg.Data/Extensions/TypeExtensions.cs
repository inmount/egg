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
    /// 类型扩展
    /// </summary>
    public static class TypeExtensions
    {

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr is null) return type.Name;
            if (tableAttr.Name.IsNullOrWhiteSpace()) return type.Name;
            return tableAttr.Name;
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static string? GetSchemaName(this Type type)
        {
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr is null) return null;
            if (tableAttr.Schema.IsNullOrWhiteSpace()) return null;
            return tableAttr.Schema;
        }

    }
}
