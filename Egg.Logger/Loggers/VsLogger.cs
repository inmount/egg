using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Egg.Log.Loggers {

    /// <summary>
    /// Vs输出
    /// </summary>
    public class VsLogger : ILogable {

        /// <summary>
        /// 输出内容
        /// </summary>
        /// <param name="entity"></param>
        public void Log(LogEntity entity)
        {
            Debug.Write($"*");
            Debug.Write($"{egg.Time.Now.ToFullDateTimeString()}");
            Debug.Write($"*");
            Debug.Write($" [{entity.Level.ToString().ToUpper()}]");
            Debug.Write($" {entity.Event} {entity.Message}");
            Debug.WriteLine("");
        }
    }
}
