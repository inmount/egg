using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 日期时间
    /// </summary>
    public class DateTimeRule : IRuleBase
    {
        // 日期时间
        private DateTimeOffset _dateTimeOffset;
        private string _format;

        /// <summary>
        /// 日期时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        public DateTimeRule(DateTimeOffset dt, string format)
        {
            _dateTimeOffset = dt;
            _format = format;
        }

        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTimeRule(string format)
        {
            _dateTimeOffset = egg.Time.Now;
            _format = format;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetValue(long index)
        {
            return _dateTimeOffset.ToString(_format);
        }

    }
}
