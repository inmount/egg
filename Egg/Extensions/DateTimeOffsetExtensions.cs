using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Egg {

    /// <summary>
    /// 时间扩展类
    /// </summary>
    public static class DateTimeOffsetExtensions {

        /// <summary>
        /// 获取yyyy-MM-dd HH:mm:ss格式时间表示字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTimeOffset dt) {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取yyyy-MM-dd格式时间表示字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTimeOffset dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取HH:mm:ss格式时间表示字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTimeOffset dt)
        {
            return dt.ToString("HH:mm:ss");
        }

    }
}
