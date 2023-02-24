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
            if (columnAttr != null)
                if (!columnAttr.TypeName.IsNullOrWhiteSpace())
                    return columnAttr.TypeName ?? "";
            var proType = pro.PropertyType;
            var proTypeCode = Type.GetTypeCode(proType);
            switch (proTypeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return "INTEGER";
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "REAL";
                case TypeCode.String:
                    return "TEXT";
                default:
                    throw new DatabaseException($"不支持的数据格式'{proTypeCode}'");
            }
        }
    }
}
