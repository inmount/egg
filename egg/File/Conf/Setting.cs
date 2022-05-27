using System;
using System.Collections.Generic;
using System.Text;

namespace egg.File.Conf {

    /// <summary>
    /// 配置项
    /// </summary>
    public class Setting : Line {

        /// <summary>
        /// 获取或设置键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            if (this.Value == null) return this.Key;
            return $"{this.Key} = {this.Value}";

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Key = null;
            this.Value = null;

            base.OnDispose();
        }

    }
}
