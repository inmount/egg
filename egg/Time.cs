using System;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 日期时间专用对象
    /// </summary>
    public class Time : egg.BasicObject {

        // 变量定义
        private DateTime val;
        private int gnTimeZone;

        /// <summary>
        /// 获取关联的时间对象
        /// </summary>
        public DateTime DateTime { get { return val; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="str"></param>
        public Time(string str) {
            Init();
            if (!DateTime.TryParse(str, out val)) {
                val = new DateTime(1970, 1, 1, 0, 0, 0);
            }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="dt"></param>
        public Time(DateTime dt) {
            Init();
            val = dt;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="tsp"></param>
        /// <param name="hasMilliseconds"></param>
        public Time(long tsp, bool hasMilliseconds = false) {
            Init();
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            if (hasMilliseconds) {
                val = dt.AddMilliseconds(tsp).AddHours(gnTimeZone);
            } else {
                val = dt.AddSeconds(tsp).AddHours(gnTimeZone);
            }

        }

        // 初始化
        private void Init() {

            // 计算时区
            DateTime dtUtc = DateTime.UtcNow;
            DateTime dtNow = DateTime.Now;

            System.TimeSpan ts = dtNow - dtUtc;
            gnTimeZone = Convert.ToInt32(ts.TotalHours);

        }

        /// <summary>
        /// 申请一个新的示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Time New(string value) {
            return new Time(value);
        }

        /// <summary>
        /// 申请一个新的示例
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Time New(DateTime dt) {
            return new Time(dt);
        }

        /// <summary>
        /// 申请一个新的示例
        /// </summary>
        /// <param name="tsp"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Time New(long tsp, bool f = false) {
            return new Time(tsp, f);
        }

        /// <summary>
        /// 申请一个新的示例
        /// </summary>
        /// <returns></returns>
        public static Time Now {
            get { return new Time(DateTime.Now); }
        }

        /// <summary>
        /// 获取指定格式的日期数据
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format) {
            return val.ToString(format);
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return val.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取标准日期字符串
        /// </summary>
        /// <returns></returns>
        public int TimeZone {
            get {
                return gnTimeZone;
            }
        }

        /// <summary>
        /// 获取标准日期字符串
        /// </summary>
        /// <returns></returns>
        public long ToTimeStamp() {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            System.TimeSpan ts = val.AddHours(0 - gnTimeZone) - dt;
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 获取包含毫秒的时间戳
        /// </summary>
        /// <returns></returns>
        public long ToMillisecondsTimeStamp() {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            System.TimeSpan ts = val.AddHours(0 - gnTimeZone) - dt;
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 获取标准日期字符串
        /// </summary>
        /// <returns></returns>
        public string ToDateString() {
            return val.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取标准时间字符串
        /// </summary>
        /// <returns></returns>
        public string ToTimeString() {
            return val.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取标准时间字符串
        /// </summary>
        /// <returns></returns>
        public string ToStringWithMilliseconds() {
            return val.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取调整后的时间
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public Time Change(int years = 0, int months = 0, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) {
            DateTime dt = val;
            if (years != 0) dt = dt.AddYears(years);
            if (months != 0) dt = dt.AddMonths(months);
            if (days != 0) dt = dt.AddDays(days);
            if (hours != 0) dt = dt.AddHours(hours);
            if (minutes != 0) dt = dt.AddMinutes(minutes);
            if (seconds != 0) dt = dt.AddSeconds(seconds);
            if (milliseconds != 0) dt = dt.AddMilliseconds(milliseconds);
            return new Time(dt);
        }

        /// <summary>
        /// 获取年份
        /// </summary>
        public int Year { get { return val.Year; } }

        /// <summary>
        /// 获取月份
        /// </summary>
        public int Month { get { return val.Month; } }

        /// <summary>
        /// 获取日期
        /// </summary>
        public int Day { get { return val.Day; } }

        /// <summary>
        /// 获取时
        /// </summary>
        public int Hour { get { return val.Hour; } }

        /// <summary>
        /// 获取分
        /// </summary>
        public int Minute { get { return val.Minute; } }

        /// <summary>
        /// 获取秒
        /// </summary>
        public int Second { get { return val.Second; } }

        /// <summary>
        /// 获取毫秒
        /// </summary>
        public int Millisecond { get { return val.Millisecond; } }

        #region [=====重载运算符=====]

        /// <summary>
        /// 一个时间加上间隔获得另外一个时间
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static Time operator +(Time t, TimeSpan ts) {
            return t.Change(milliseconds: ts.Milliseconds);
        }

        /// <summary>
        /// 一个时间减去间隔获得另外一个时间
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static Time operator -(Time t, TimeSpan ts) {
            return t.Change(milliseconds: 0 - ts.Milliseconds);
        }

        /// <summary>
        /// 计算两个时间差
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static TimeSpan operator -(Time t1, Time t2) {
            return new TimeSpan(t1, t2);
        }

        #endregion

    }
}
