using System;
using System.Collections.Generic;
using System.Text;

namespace Egg {

    /// <summary>
    /// 全字符串字典
    /// </summary>
    public class StringKeyValues : StringKeyDictionary<string> {

        /// <summary>
        /// 从字典中复制内容
        /// </summary>
        /// <param name="pairs"></param>
        public new void MapFrom(Dictionary<string, string> pairs)
        {
            foreach (var item in pairs) {
                this[item.Key] = item.Value;
            }
        }

    }
}
