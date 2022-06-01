using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// 包含内容的Markdown元素的基础块对象
    /// </summary>
    public abstract class MdBasicBlock : MdBasic {

        /// <summary>
        /// 获取子元素集合
        /// </summary>
        public MdBasicCollection Children { get; private set; }

        /// <summary>
        /// 获取或设置层次表示字符串
        /// </summary>
        public string LevelString { get; private set; }

        /// <summary>
        /// 对象实例化
        /// <param name="mdType"></param>
        /// <param name="lvString"></param>
        /// </summary>
        public MdBasicBlock(MdTypes mdType, string lvString = "") : base(mdType) {
            this.Children = new MdBasicCollection(this);
            this.LevelString = lvString;
        }

        /// <summary>
        /// 获取完整的分层字符串
        /// </summary>
        /// <returns></returns>
        public string GetSpace() {
            if (this.ParentBlock.IsNull()) {
                return "";
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.ParentBlock.GetSpace());
                sb.Append("  ");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取完整的分层字符串
        /// </summary>
        /// <returns></returns>
        public string GetFullLevelString() {
            if (this.ParentBlock.IsNull()) {
                return this.LevelString;
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.ParentBlock.GetFullLevelString());
                sb.Append(this.LevelString);
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取标准的Markdown字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return this.Children.GetMarkdownString();
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[BasicBlock {0}]", this.Children.Count);
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return this.Children.GetHtmlString();
        }

    }
}
