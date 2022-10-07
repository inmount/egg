using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Console {

    /// <summary>
    /// 参数集合
    /// 详细格式1：/key value
    /// 详细格式2：-key value
    /// </summary>
    public class Arguments : egg.KeyStrings, IParams {

        private string[] _tags;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Arguments() {
            _tags = new string[] { "-", "/" };
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetParam(string key) {
            return this[key];
        }

        /// <summary>
        /// 设置命令行参数
        /// </summary>
        /// <param name="args"></param>
        public void SetParams(string[] args) {
            string sign = null;
            string value = null;
            string[] tags = _tags;
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

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void IParams.SetParam(string key, string value) {
            this[key] = value;
        }
    }
}
