using System;
using System.Diagnostics;
using egg;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// 调试入口
    /// </summary>
    public static class Debug {

        /// <summary>
        /// 输出一行
        /// </summary>
        /// <param name="cnt"></param>
        public static void WriteLine(string cnt) {
            Write(cnt, true, true);
        }

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="cnt"></param>
        public static void Write(string cnt) {
            Write(cnt, false, false);
        }

        /// <summary>
        /// 带标志输出
        /// </summary>
        /// <param name="cnt"></param>
        public static void WriteWithSign(string cnt) {
            Write(cnt, true, false);
        }

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="sign"></param>
        /// <param name="r"></param>
        public static void Write(string cnt, bool sign, bool r) {
            if (sign) cnt = "[" + eggs.Time.GetNowTime().ToString() + @"] \>" + cnt;
            if (r) {
                Trace.WriteLine(cnt);
            } else {
                Trace.Write(cnt);
            }
        }

    }
}
