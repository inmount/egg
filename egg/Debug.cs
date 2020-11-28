using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace egg {

    /// <summary>
    /// 调试输出专用类
    /// </summary>
    public class Debug {

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
            if (sign) cnt = "[" + eggs.GetNowTime().ToString() + @"] \>" + cnt;
            if (r) {
                Trace.WriteLine(cnt);
            } else {
                Trace.Write(cnt);
            }
        }

    }
}
