using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Values {
    /// <summary>
    /// 空值
    /// </summary>
    public class None : Value {

        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckEmpty() {
            return true;
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return "[None]";
        }

    }
}
