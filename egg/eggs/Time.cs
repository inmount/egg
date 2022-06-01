using System;
using egg;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// 时间组件
    /// </summary>
    public static class Time {

        /// <summary>
        /// 获取当前时间管理器
        /// </summary>
        /// <returns></returns>
        public static egg.Time GetNow() { return egg.Time.Now; }

        /// <summary>
        /// 获取当前时间字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNowString(string format = "yyyy-MM-dd HH:mm:ss") { return egg.Time.Now.ToString(format); }

        /// <summary>
        /// 获取当前日期字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNowDate() { return egg.Time.Now.ToDateString(); }

        /// <summary>
        /// 获取当前日期字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNowTime() { return egg.Time.Now.ToTimeString(); }

        /// <summary>
        /// 获取时间管理器
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static egg.Time GetTime(DateTime dt) { return new egg.Time(dt); }

        /// <summary>
        /// 获取时间管理器
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static egg.Time GetTime(string str) { return new egg.Time(str); }

        /// <summary>
        /// 获取时间管理器
        /// </summary>
        /// <param name="tsp"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static egg.Time GetTime(long tsp, bool f = false) { return new egg.Time(tsp, f); }

        /// <summary>
        /// 获取当前时间间隔(秒)
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long GetTimeSpan(egg.Time time1, egg.Time time2) {
            return time1.ToTimeStamp() - time2.ToTimeStamp();
        }

        /// <summary>
        /// 获取当前时间间隔(秒)
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long GetTimeSpan(string time1, string time2) { return GetTimeSpan(GetTime(time1), GetTime(time2)); }

        /// <summary>
        /// 获取当前时间间隔(秒)
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long GetTimeSpan(egg.Time time1, string time2) { return GetTimeSpan(time1, GetTime(time2)); }

        /// <summary>
        /// 获取当前时间间隔(秒)
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long GetTimeSpan(string time1, egg.Time time2) { return GetTimeSpan(GetTime(time1), time2); }

    }
}
