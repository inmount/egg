using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Log {

    /// <summary>
    /// 记录器接口
    /// </summary>
    public interface ILogable {

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        void Log(LogEntity entity);

    }
}
