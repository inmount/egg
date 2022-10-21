using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 随机字符串
    /// </summary>
    public class RandomStringRule : IRuleBase
    {
        // 值
        private string _keys;
        private int _len;

        /// <summary>
        /// 实例化一个
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="len"></param>
        public RandomStringRule(string keys, int len)
        {
            _keys = keys;
            _len = len;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetValue(long index)
        {
            Random rnd = egg.BarCodeGenerator.Random;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _len; i++) sb.Append(_keys[rnd.Next(_keys.Length)]);
            return sb.ToString();
        }

    }
}
