using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FuncAttribute : System.Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 函数定义
        /// </summary>
        public FuncAttribute() { }

        /// <summary>
        /// 函数定义
        /// </summary>
        /// <param name="name">名称</param>
        public FuncAttribute(string name) { this.Name = name; }
    }
}
