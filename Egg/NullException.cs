using System;
using System.Collections.Generic;
using System.Text;

namespace Egg
{
    /// <summary>
    /// 为空错误
    /// </summary>
    public class NullException : Exception
    {
        /// <summary>
        /// 为空错误
        /// </summary>
        public NullException() : base("对象为空") { }
    }
}
