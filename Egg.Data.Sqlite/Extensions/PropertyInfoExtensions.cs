using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Egg.Data.Sqlite.Extensions
{
    /// <summary>
    /// 属性扩展
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 获取列属性类型
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static string GetColumnAttributeType(this PropertyInfo pro)
        {
            var columnAttr = pro.GetCustomAttribute<ColumnAttribute>();
            if (!columnAttr.TypeName.IsNullOrWhiteSpace()) return columnAttr.TypeName ?? "";
            var proType = pro.PropertyType;
            string proTypeName = proType.GetTopName();
            switch (proTypeName)
            {
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt32":
                case "System.UInt64":
                    return "INTEGER";
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    return "REAL";
                case "System.String":
                    return "TEXT";
                default:
                    throw new DatabaseException($"不支持的数据格式'{proTypeName}'");
            }
        }
    }
}
