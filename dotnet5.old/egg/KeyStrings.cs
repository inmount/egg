using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 键/值字符串集合
    /// </summary>
    public class KeyStrings : KeyList<string> {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public KeyStrings() { }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyStrings Set(string key, string value) {
            this[key] = value;
            return this;
        }

        /// <summary>
        /// 实例化一个键/值字符串集合
        /// </summary>
        /// <returns></returns>
        public static KeyStrings Create() { return new KeyStrings(); }

    }
}
