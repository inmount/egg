using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Egg.EFCore
{
    /// <summary>
    /// 更新器属性
    /// </summary>
    public class UpdaterProperty
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

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
        /// 获取Sql中的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSqlValue(object obj)
        {
            var value = this.PropertyInfo.GetValue(obj);
            if (value is null) return "null";
            if (IsNumeric) return value.ToString().ToLower();
            return GetSafetySqlString(value.ToString());
        }

        // 获取类型名称
        private string GetTypeName(Type type)
        {
            string name = type.Namespace + "." + type.Name;
            // 兼容可空类型
            if (name == "System.Nullable`1") return GetTypeName(type.GenericTypeArguments[0]);
            return name;
        }

        /// <summary>
        /// 更新器属性
        /// </summary>
        public UpdaterProperty(PropertyInfo property)
        {
            this.PropertyInfo = property;
            this.Name = property.Name;
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
