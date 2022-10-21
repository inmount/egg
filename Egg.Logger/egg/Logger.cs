using Egg.Log;
using System;
using System.Collections.Generic;
using System.Text;
using static Egg.Log.LoggerCollection;

namespace egg
{

    /// <summary>
    /// 日志管理器
    /// </summary>
    public static class Logger
    {

        private static LoggerCollection? _loggers;

        /// <summary>
        /// 注册一个日志委托
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static LoggerCollection Reg(LogMessageHandle log)
        {
            _loggers = _loggers ?? new LoggerCollection();
            _loggers.AddHandle(log);
            return _loggers;
        }

        /// <summary>
        /// 注册一个日志委托
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static LoggerCollection Reg(LogEntityHandle log)
        {
            _loggers = _loggers ?? new LoggerCollection();
            _loggers.AddHandle(log);
            return _loggers;
        }

        /// <summary>
        /// 使用一个日志管理器
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LoggerCollection Reg(ILogable logger)
        {
            _loggers = _loggers ?? new LoggerCollection();
            _loggers.Add(logger);
            return _loggers;
        }

        /// <summary>
        /// 使用一个日志管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LoggerCollection Use<T>() where T : ILogable, new()
        {
            {
                _loggers = _loggers ?? new LoggerCollection();
                _loggers.Use<T>();
                return _loggers;
            }
        }

        /// <summary>
        /// 添加一条调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public static void Debug(string message, string evt = "default")
        {
            _loggers?.Log(new LogEntity()
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
            _loggers?.Log(new LogEntity()
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
            _loggers?.Log(new LogEntity()
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
            _loggers?.Log(new LogEntity()
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
            _loggers?.Log(new LogEntity()
            {
                Event = evt,
                Level = LogLevel.Fatal,
                Message = message
            });
        }
    }
}
