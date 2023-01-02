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
        /// 更新器属性
        /// </summary>
        public UpdaterProperty(PropertyInfo property)
        {
            this.Name = property.Name;
            this.ColumnName = property.Name;
            var column = property.GetCustomAttribute<ColumnAttribute>();
            if (column != null)
            {
                if (!string.IsNullOrWhiteSpace(column.Name)) this.ColumnName = column.Name;
            }
            this.IsModified = false;
        }
    }
}
