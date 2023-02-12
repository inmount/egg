using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data
{
    /// <summary>
    /// 数据库处理异常
    /// </summary>
    public class DatabaseException : Exception
    {
        /// <summary>
        /// 数据库处理异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public DatabaseException(string message) : base(message) { }

        /// <summary>
        /// 数据库处理异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">包含的异常</param>
        public DatabaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
