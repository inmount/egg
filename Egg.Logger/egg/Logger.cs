using Egg.Log;
using System;
using System.Collections.Generic;
using System.Text;
using static Egg.Log.Logger;

namespace egg
{

    /// <summary>
    /// 日志管理器
    /// </summary>
    public static class Logger
    {

        private static Egg.Log.Logger? _loggers;

        /// <summary>
        /// 获取当前日志记录器
        /// </summary>
        /// <returns></returns>
        public static Egg.Log.Logger GetCurrentLogger()
        {
            if (_loggers is null) _loggers = new Egg.Log.Logger();
            return _loggers;
        }

        /// <summary>
        /// 使用一个新的日志记录器
        /// </summary>
        /// <returns></returns>
        public static Egg.Log.Logger Create()
        {
            _loggers = new Egg.Log.Logger();
            return _loggers;
        }

        /// <summary>
        /// 添加一条调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Debug(string message, string evt = "default")
        {
            _loggers?.Log(new LogInfo()
            {
                Event = evt,
                Level = LogLevel.Debug,
                Message = message
            });
        }

        /// <summary>
        /// 添加一条普通信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Info(string message, string evt = "default")
        {
            _loggers?.Log(new LogInfo()
            {
                Event = evt,
                Level = LogLevel.Info,
                Message = message
            });
        }

        /// <summary>
        /// 添加一条警告信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Warn(string message, string evt = "default")
        {
            _loggers?.Log(new LogInfo()
            {
                Event = evt,
                Level = LogLevel.Warn,
                Message = message
            });
        }

        /// <summary>
        /// 添加一条错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Error(string message, string evt = "default")
        {
            _loggers?.Log(new LogInfo()
            {
                Event = evt,
                Level = LogLevel.Error,
                Message = message
            });
        }

        /// <summary>
        /// 添加一条致命错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Fatal(string message, string evt = "default")
        {
            _loggers?.Log(new LogInfo()
            {
                Event = evt,
                Level = LogLevel.Fatal,
                Message = message
            });
        }
    }
}
