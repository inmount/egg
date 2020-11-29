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
        public Arguments(string[] args = null) {

            string sign = null;
            string value = null;

            // 获取参数
            for (int i = 0; i < args.Length; i++) {

                string argSign = args[i];
                if (argSign.StartsWith("-") || argSign.StartsWith("/")) {

                    // 判断是否存在未存储的内容
                    if (!sign.IsNoneOrNull()) {
                        // 存储之前的参数定义
                        this[sign] = value;
                        value = null;
                    }

                    // 缓存标志
                    sign = argSign.Substring(1);

                } else {

                    // 填充内容
                    if (!value.IsNoneOrNull()) value += " ";
                    value += argSign;

                }
            }

            // 判断是否存在未存储的内容
            if (!sign.IsNoneOrNull()) {
                // 存储之前的参数定义
                this[sign] = value;
                value = null;
            }

        }

    }
}
