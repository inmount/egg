using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Egg.Log.Loggers
{

    /// <summary>
    /// 可注册方法日志管理器
    /// </summary>
    public class ActionInfoLogger : ILogable
    {
        // 方法
        private readonly Action<LogInfo> _action;

        /// <summary>
        /// 可注册方法日志管理器
        /// </summary>
        /// <param name="action"></param>
        public ActionInfoLogger(Action<LogInfo> action)
        {
            _action = action;
        }

        /// <summary>
        /// 输出内容
        /// </summary>
        /// <param name="info"></param>
        public void Log(LogInfo info)
        {
            _action(info);
        }
    }
}
