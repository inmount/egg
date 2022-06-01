using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Console {

    /// <summary>
    /// 参数集合
    /// </summary>
    public class Arguments : egg.KeyList<string> {

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="args"></param>
        /// <param name="tags"></param>
        public Arguments(string[] args = null, string[] tags = null) {

            string sign = null;
            string value = null;
            if (eggs.Object.IsNull(tags)) tags = new string[] { "-", "/" };

            // 获取参数
            for (int i = 0; i < args.Length; i++) {
                string argSign = args[i];
                // 判断是否满足标签定义
                bool isTag = false;
                for (int j = 0; j < tags.Length; j++) {
                    if (argSign.StartsWith(tags[j])) {
                        // 判断是否存在未存储的内容
                        if (!sign.IsEmpty()) {
                            // 存储之前的参数定义
                            this[sign] = value;
                            value = null;
                        }
                        // 缓存标志
                        sign = argSign.Substring(tags[j].Length);
                        isTag = true;
                        break;
                    }
                }
                if (!isTag) {
                    // 填充内容
                    if (!value.IsEmpty()) value += " ";
                    value += argSign;
                }
            }

            // 判断是否存在未存储的内容
            if (!sign.IsEmpty()) {
                // 存储之前的参数定义
                this[sign] = value;
                value = null;
            }

        }

    }
}
