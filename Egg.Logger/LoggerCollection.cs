using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Egg.Log
{

    /// <summary>
    /// 日志器集合
    /// </summary>
    public class LoggerCollection : List<ILogable>
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
        public delegate void LogEntityHandle(LogEntity message);

        // 所有日志输出接口
        private List<LogMessageHandle> _logMessages;
        private List<LogEntityHandle> _logEntities;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public LoggerCollection()
        {
            _logMessages = new List<LogMessageHandle>();
            _logEntities = new List<LogEntityHandle>();
        }

        /// <summary>
        /// 注册使用日志器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public LoggerCollection Use<T>() where T : ILogable, new()
        {
            this.Add(new T());
            return this;
        }

        /// <summary>
        /// 使用日志器
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public LoggerCollection Reg(ILogable logger)
        {
            this.Add(logger);
            return this;
        }

        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public void AddHandle(LogMessageHandle logger)
        {
            _logMessages.Add(logger);
        }

        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public void AddHandle(LogEntityHandle logger)
        {
            _logEntities.Add(logger);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="level"></param>
        /// <param name="evt"></param>
        /// <param name="message"></param>
        public void Log(LogEntity entity)
        {
            string content = $"*{egg.Time.Now.ToTimeString()}* [{entity.Level.ToString().ToUpper()}] {entity.Event} {entity.Message}";
            // 输出到委托
            for (int i = 0; i < _logMessages.Count; i++)
            {
                try { _logMessages[i](content); } catch { }
            }
            // 输出到所有记录器
            for (int i = 0; i < base.Count; i++)
            {
                try { base[i].Log(entity); } catch { }
            }
        }

    }
}
