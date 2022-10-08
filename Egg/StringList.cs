using System;
using System.Collections.Generic;

namespace Egg {

    /// <summary>
    /// 字符串列表
    /// </summary>
    public class StringList : List<string> {

        /// <summary>
        /// 创建一个字符串列表
        /// </summary>
        public StringList() { }

        /// <summary>
        /// 创建一个字符串列表
        /// </summary>
        /// <param name="args"></param>
        public StringList(params string[] args) {
            base.AddRange(args);
        }

        /// <summary>
        /// 从列表中复制内容
        /// </summary>
        /// <param name="strings"></param>
        public void MapFrom(List<string> strings) {
            this.AddRange(strings);
        }

    }

}
