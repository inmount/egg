using System;
using System.Collections.Generic;
using System.Text;

namespace Egg {

    /// <summary>
    /// 字符串索引字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringKeyDictionary<T> : Dictionary<string, T> {

        /// <summary>
        /// 获取是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() { return this.Count == 0; }

        /// <summary>
        /// 从字典中复制内容
        /// </summary>
        /// <param name="pairs"></param>
        public void MapFrom(Dictionary<string, T> pairs) {
            foreach (var item in pairs) {
                this[item.Key] = item.Value;
            }
        }

    }
}
