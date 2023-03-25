using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Egg.Log
{

    /// <summary>
    /// 日志器集合
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// 日志输出接口
        /// </summary>
        /// <param name="content"></param>
        public delegate void LogMessageHandle(string? message);

        /// <summary>
        /// 日志对象输出接口
        /// </summary>
        /// <param name="content"></param>
        public delegate void LogInfoHandle(LogInfo message);

        // 所有日志输出接口
        private readonly IList<Action<string>> _logMessages;
        private readonly IList<Action<LogInfo>> _logInfos;
        private readonly IList<ILogable> _loggers;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Logger()
        {
            _logMessages = new List<Action<string>>();
            _logInfos = new List<Action<LogInfo>>();
            _loggers = new List<ILogable>();
        }

        /// <summary>
        /// 注册使用日志器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Logger Use<T>() where T : ILogable, new()
        {
            _loggers.Add(new T());
            return this;
        }

        /// <summary>
        /// 使用日志器
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public Logger Use(ILogable logger)
        {
            _loggers.Add(logger);
            return this;
        }

        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public Logger Use(Action<string> logger)
        {
            _logMessages.Add(logger);
            return this;
        }

        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public Logger Use(Action<LogInfo> logger)
        {
            _logInfos.Add(logger);
            return this;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="info"></param>
        public void Log(LogInfo info)
        {
            string content = $"*{egg.Time.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}* [{info.Level.ToString().ToUpper()}] {info.Event} {info.Message}";
            // 输出到委托
            for (int i = 0; i < _logMessages.Count; i++)
            {
                try { _logMessages[i](content); } catch { }
            }
            // 输出到委托
            for (int i = 0; i < _logInfos.Count; i++)
            {
                try { _logInfos[i](info); } catch { }
            }
            // 输出到所有记录器
            for (int i = 0; i < _loggers.Count; i++)
            {
                try { _loggers[i].Log(info); } catch { }
            }
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public void Debug(string message, string? evt = null)
            => Log(new LogInfo() { Event = evt, Level = LogLevel.Debug, Message = message });

        /// <summary>
        /// 记录普通信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public void Info(string message, string? evt = null)
            => Log(new LogInfo() { Event = evt, Level = LogLevel.Info, Message = message });

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public void Warn(string message, string? evt = null)
            => Log(new LogInfo() { Event = evt, Level = LogLevel.Warn, Message = message });

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public void Error(string message, string? evt = null)
            => Log(new LogInfo() { Event = evt, Level = LogLevel.Error, Message = message });

        /// <summary>
        /// 记录致命信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        public void Fatal(string message, string? evt = null)
            => Log(new LogInfo() { Event = evt, Level = LogLevel.Fatal, Message = message });
    }
}
