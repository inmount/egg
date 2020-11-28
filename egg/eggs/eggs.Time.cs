using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 获取当前时间管理器
    /// </summary>
    /// <returns></returns>
    public static Time GetNow() { return Time.Now; }

    /// <summary>
    /// 获取当前时间字符串
    /// </summary>
    /// <returns></returns>
    public static string GetNowString(string format = "yyyy-MM-dd HH:mm:ss") { return Time.Now.ToString(format); }

    /// <summary>
    /// 获取当前日期字符串
    /// </summary>
    /// <returns></returns>
    public static string GetNowDate() { return Time.Now.ToDateString(); }

    /// <summary>
    /// 获取当前日期字符串
    /// </summary>
    /// <returns></returns>
    public static string GetNowTime() { return Time.Now.ToTimeString(); }

    /// <summary>
    /// 获取时间管理器
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Time GetTime(DateTime dt) { return new Time(dt); }

    /// <summary>
    /// 获取时间管理器
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Time GetTime(string str) { return new Time(str); }

    /// <summary>
    /// 获取时间管理器
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Time GetTime(long tsp, bool f = false) { return new Time(tsp, f); }

    /// <summary>
    /// 获取当前时间间隔(秒)
    /// </summary>
    /// <param name="time1"></param>
    /// <param name="time2"></param>
    /// <returns></returns>
    public static long GetTimeSpan(Time time1, Time time2) {
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
    public static long GetTimeSpan(Time time1, string time2) { return GetTimeSpan(time1, GetTime(time2)); }

    /// <summary>
    /// 获取当前时间间隔(秒)
    /// </summary>
    /// <param name="time1"></param>
    /// <param name="time2"></param>
    /// <returns></returns>
    public static long GetTimeSpan(string time1, Time time2) { return GetTimeSpan(GetTime(time1), time2); }

}
