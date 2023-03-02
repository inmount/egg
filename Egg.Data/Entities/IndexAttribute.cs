using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data.Entities
{
    /// <summary>
    /// 索引字段
    /// </summary>
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 唯一性
        /// </summary>
        public bool Unique { get; set; } = false;

        /// <summary>
        /// 索引字段
        /// </summary>
        public IndexAttribute() { this.Name = string.Empty; }

        /// <summary>
        /// 索引字段
        /// </summary>
        public IndexAttribute(string name) { this.Name = name; }
    }
}
