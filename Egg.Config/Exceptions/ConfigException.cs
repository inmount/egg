using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Config.Exceptions
{
    /// <summary>
    /// 配置文件异常
    /// </summary>
    public class ConfigException : Exception
    {
        /// <summary>
        /// 配置文件异常
        /// </summary>
        /// <param name="message">消息</param>
        public ConfigException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
