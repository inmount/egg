using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 包含内容的Markdown元素的基础对象
    /// </summary>
    public abstract class MdBasicContent : MdBasic {

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 对象实例化
        /// <param name="mdType"></param>
        /// </summary>
        public MdBasicContent(MdTypes mdType) : base(mdType) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return this.Content;
        }

    }
}
