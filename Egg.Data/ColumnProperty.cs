using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Egg.Data
{
    /// <summary>
    /// 字段属性
    /// </summary>
    public class ColumnProperty
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string VarName { get; }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 是否更新
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// 获取是否为数字
        /// </summary>
        public bool IsNumeric { get; }

        /// <summary>
        /// 获取安全的sql字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSafetySqlString(string str)
        {
            return "'" + str.Replace("'", "''") + "'";
        }

        /// <summary>
        /// 判断是否数字类型类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type type)
        {
            string typeFullName = GetTypeName(type);
            switch (typeFullName)
            {
                case "System.Byte":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Boolean":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取Sql中的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSqlValue(object obj)
        {
            var value = this.PropertyInfo.GetValue(obj);
            if (value is null) return "NULL";
            if (IsNumeric) return value.ToString().ToUpper();
            return GetSafetySqlString(value.ToString());
        }

        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            string name = type.Namespace + "." + type.Name;
            // 兼容可空类型
            if (name == "System.Nullable`1") return GetTypeName(type.GenericTypeArguments[0]);
            return name;
        }

        /// <summary>
        /// 更新器属性
        /// </summary>
        public ColumnProperty(PropertyInfo property)
        {
            this.PropertyInfo = property;
            this.VarName = property.Name;
            this.ColumnName = property.Name;
            var column = property.GetCustomAttribute<ColumnAttribute>();
            if (column != null)
            {
                if (!string.IsNullOrWhiteSpace(column.Name)) this.ColumnName = column.Name;
            }
            this.IsModified = false;
            string typeFullName = GetTypeName(property.PropertyType);
            switch (typeFullName)
            {
                case "System.Byte":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Int32":
                case "System.Int64":
                case "System.Boolean":
                    IsNumeric = true;
                    break;
                default:
                    IsNumeric = false;
                    break;
            }
        }
    }
}
