using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 字符串集合
    /// </summary>
    public class Strings : List<string> {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Strings() { }

        /// <summary>
        /// 实例化一个键/值字符串集合
        /// </summary>
        /// <returns></returns>
        public static Strings Create(params string[] args) {
            Strings strs = new Strings();
            for (int i = 0; i < args.Length; i++) strs.Add(args[i]);
            return strs;
        }

    }
}
