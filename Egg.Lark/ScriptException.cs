using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 脚本异常
    /// </summary>
    public class ScriptException : System.Exception
    {
        /// <summary>
        /// 脚本异常
        /// </summary>
        /// <param name="message">错误信息</param>
        public ScriptException(string message) : base(message) { }
    }
}
