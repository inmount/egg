using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// Markdown文档对象
    /// </summary>
    public class Document : Serializable.Document {

        /// <summary>
        /// 获取根节点
        /// </summary>
        public MdDocumentRoot Root { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Document() {
            this.Root = null;
        }

        // 反序列化内容填充
        protected override void OnDeserialize(Span<byte> bytes) {
            if (bytes.Length > 0) {
                this.Root = eggs.Markdown.GetDocument(System.Text.Encoding.UTF8.GetString(bytes));
            } else {
                this.Root = new MdDocumentRoot();
            }
        }

        // 内容序列化到字节数组
        protected override byte[] OnSerializeToBytes() {
            return System.Text.Encoding.UTF8.GetBytes(this.Root.ToMarkdown());
        }

        // 释放资源
        protected override void OnDispose() {
            this.Root.Destroy();
            base.OnDispose();
        }

    }
}
