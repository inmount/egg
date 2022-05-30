﻿using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// Markdown 文档对象
    /// </summary>
    public class MdDocumentRoot : MdBasicBlock {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdDocumentRoot() : base(MdTypes.DocumentRoot) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Children.Count; i++) {
                sb.AppendFormat("{0}. {1}\r\n", (i + 1).ToString().PadLeft(4, ' '), this.Children[i].ToString());
            }
            return sb.ToString();
        }

    }
}
