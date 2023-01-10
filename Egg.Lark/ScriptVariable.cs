using System;
using System.Collections.Generic;

namespace Egg.Lark
{
    /// <summary>
    /// 变量
    /// </summary>
    public class ScriptVariable : IDisposable
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
