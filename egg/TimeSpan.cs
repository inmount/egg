using System;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 日期时间专用对象
    /// </summary>
    public class TimeSpan : egg.BasicObject {

        // 时间对毫秒的转化
        private const int MS_Second = 1000;
        private const int MS_Minute = MS_Second * 60;
        private const int MS_Hour = MS_Minute * 60;
        private const int MS_Day = MS_Hour * 24;

        // 记录毫秒数
        private int ts;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public TimeSpan(Time t1, Time t2) {
            ts = (int)(t1.ToMillisecondsTimeStamp() - t2.ToMillisecondsTimeStamp());
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="ms"></param>
        public TimeSpan(int ms = 0) {
            ts = ms;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        public TimeSpan(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) {
            ts = 0;
            if (days != 0) ts += days * MS_Day;
            if (hours != 0) ts += hours * MS_Hour;
            if (minutes != 0) ts += minutes * MS_Minute;
            if (seconds != 0) ts += seconds * MS_Second;
            if (milliseconds != 0) ts += milliseconds;
        }

        /// <summary>
        /// 申请一个新的示例
        /// </summary>
        /// <returns></returns>
        public static Time Now {
            get { return new Time(DateTime.Now); }
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            long ms = ts;
            // 先计算正负
            if (ms > 0) {
                sb.Append("+ ");
            } else {
                sb.Append("- ");
                ms *= -1;
            }
            // 计算天数
            if (ms > MS_Day) {
                sb.Append(ms / MS_Day);
                sb.Append(" days ");
                ms = ms % MS_Day;
            }
            // 计算小时数
            sb.Append(ms / MS_Hour);
            ms = ms % MS_Hour;
            // 计算分钟数
            sb.Append(":");
            sb.Append((ms / MS_Minute).ToString().PadLeft(2, '0'));
            ms = ms % MS_Minute;
            // 计算秒数
            sb.Append(":");
            sb.Append((ms / MS_Second).ToString().PadLeft(2, '0'));
            ms = ms % MS_Second;
            // 添加毫秒数
            sb.Append(":");
            sb.Append(ms.ToString().PadLeft(3, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// 获取调整后的时间
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        public void Change(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) {
            if (days != 0) ts += days * MS_Day;
            if (hours != 0) ts += hours * MS_Hour;
            if (minutes != 0) ts += minutes * MS_Minute;
            if (seconds != 0) ts += seconds * MS_Second;
            if (milliseconds != 0) ts += milliseconds;
        }

        /// <summary>
        /// 获取调整后的时间
        /// </summary>
        /// <param name="tsp"></param>
        public void Change(TimeSpan tsp) {
            ts += tsp.Milliseconds;
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        public int Days { get { return ts / MS_Day; } }

        /// <summary>
        /// 获取时
        /// </summary>
        public int Hours { get { return ts / MS_Hour; } }

        /// <summary>
        /// 获取分
        /// </summary>
        public int Minutes { get { return ts / MS_Minute; } }

        /// <summary>
        /// 获取秒
        /// </summary>
        public int Seconds { get { return ts / MS_Second; } }

        /// <summary>
        /// 获取毫秒
        /// </summary>
        public int Milliseconds { get { return ts; } }

        #region [=====重载运算符=====]

        /// <summary>
        /// 两个时间间隔相加
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static TimeSpan operator +(TimeSpan t1, TimeSpan t2) {
            return new TimeSpan(t1.Milliseconds + t2.Milliseconds);
        }

        /// <summary>
        /// 两个时间间隔相减
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static TimeSpan operator -(TimeSpan t1, TimeSpan t2) {
            return new TimeSpan(t1.Milliseconds - t2.Milliseconds);
        }

        #endregion

    }
}
