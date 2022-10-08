using System;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 时间快捷操作类
    /// </summary>
    public static class Time {

        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static DateTimeOffset Now { get { return DateTimeOffset.Now; } }

        /// <summary>
        /// 从字符串加载时间
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static DateTimeOffset Parse(string sz) { return DateTimeOffset.Parse(sz); }

        /// <summary>
        /// 从时间戳加载时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTimeOffset Parse(long timestamp) { return DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime(); }
    }
}
