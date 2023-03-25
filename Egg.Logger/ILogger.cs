using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Log
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public interface ILogger : ILogable
    {
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="evt">事件源</param>
        void Debug(string message, string? evt = null);

        /// <summary>
        /// 记录普通信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="evt">事件源</param>
        void Info(string message, string? evt = null);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        void Warn(string message, string? evt = null);

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        void Error(string message, string? evt = null);

        /// <summary>
        /// 记录致命信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="evt"></param>
        void Fatal(string message, string? evt = null);
    }
}
