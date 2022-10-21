using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 固定字符串
    /// </summary>
    public class FixedString : IRuleBase
    {
        // 值
        private string _value;

        /// <summary>
        /// 实例化一个
        /// </summary>
        /// <param name="value"></param>
        public FixedString(string value)
        {
            _value = value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetValue(long index)
        {
            return _value;
        }

    }
}
