using System;
using System.Collections.Generic;
using System.Text;

namespace egg.File.Conf {

    /// <summary>
    /// 注释
    /// </summary>
    public class Note : Line {

        /// <summary>
        /// 获取或设置注释内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return $"#{this.Content}";
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Content = null;

            base.OnDispose();
        }

    }
}
