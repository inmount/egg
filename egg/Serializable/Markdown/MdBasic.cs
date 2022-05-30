using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// Markdown的元素类型
    /// </summary>
    public enum MdTypes {

        /// <summary>
        /// 空
        /// </summary>
        Null = 0x00,

        /// <summary>
        /// 分隔线
        /// </summary>
        Line = 0x10,

        /// <summary>
        /// 纯文本
        /// </summary>
        Text = 0x20,

        /// <summary>
        /// 文本行
        /// </summary>
        TextLine = 0x21,

        /// <summary>
        /// 超链接
        /// </summary>
        TextLink = 0x22,

        /// <summary>
        /// 列表
        /// </summary>
        List = 0x30,

        /// <summary>
        /// 列表项
        /// </summary>
        ListItem = 0x31,

        /// <summary>
        /// 区块
        /// </summary>
        Block = 0x40,

        /// <summary>
        /// 表
        /// </summary>
        Table = 0x50,

        /// <summary>
        /// 表行
        /// </summary>
        TableRow = 0x51,

        /// <summary>
        /// 表头
        /// </summary>
        TableHeader = 0x52,

        /// <summary>
        /// 表单元格
        /// </summary>
        TableCell = 0x53,

        /// <summary>
        /// 表对齐
        /// </summary>
        TableAlign = 0x54,

        /// <summary>
        /// 代码行
        /// </summary>
        CodeLine = 0x60,

        /// <summary>
        /// 代码块
        /// </summary>
        CodeBlock = 0x60,

        /// <summary>
        /// 标题
        /// </summary>
        Title = 0x70,

        /// <summary>
        /// 文档根对象
        /// </summary>
        DocumentRoot = 0xFFFF,


    }

    /// <summary>
    /// Markdown的基础对象
    /// </summary>
    public abstract class MdBasic : egg.BasicObject {

        /// <summary>
        /// 获取父区块
        /// </summary>
        public MdBasicBlock ParentBlock { get; internal set; }

        /// <summary>
        /// 获取元素类型
        /// </summary>
        public MdTypes Type { get; private set; }

        /// <summary>
        /// 对象实例化
        /// <param name="mdType"></param>
        /// </summary>
        public MdBasic(MdTypes mdType) {
            this.Type = mdType;
            this.ParentBlock = null;
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[Basic]");
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetHtmlString() { return ""; }

        /// <summary>
        /// 转化为HTML代码
        /// </summary>
        /// <returns></returns>
        public string ToHtml() { return OnGetHtmlString(); }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetMarkdownString() { return ""; }

        /// <summary>
        /// 转化为Markdown代码
        /// </summary>
        /// <returns></returns>
        public string ToMarkdown() { return OnGetMarkdownString(); }

    }
}
