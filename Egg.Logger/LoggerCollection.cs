using System;
using System.Collections.Generic;

namespace Egg.Log {

    /// <summary>
    /// 日志器集合
    /// </summary>
    public class LoggerCollection : List<ILogable> {

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
        /// 日志记录
        /// </summary>
        /// <param name="level"></param>
        /// <param name="evt"></param>
        /// <param name="message"></param>
        public void Log(LogEntity entity)
        {
            for (int i = 0; i < base.Count; i++) {
                base[i].Log(entity);
            }
        }

    }
}
