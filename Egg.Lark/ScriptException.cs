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
        /// <param name="message">消息</param>
        /// <param name="innerException">内联异常</param>
        public ScriptException(string message, Exception? innerException = null) : base(message, innerException) { }

   }
}
