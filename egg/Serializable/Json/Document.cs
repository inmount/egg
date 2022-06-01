﻿using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// Json文档对象
    /// </summary>
    public class Document : Serializable.Document {

        /// <summary>
        /// 获取根节点
        /// </summary>
        public Node RootNode { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Document() {
            this.RootNode = null;
        }

        /// <summary>
        /// 反序列化内容填充
        /// </summary>
        /// <param name="bytes"></param>
        protected override void OnDeserialize(Span<byte> bytes) {
            if (bytes.Length > 0) {
                this.RootNode = eggs.Json.Parse(System.Text.Encoding.UTF8.GetString(bytes));
            } else {
                this.RootNode = new egg.Serializable.Json.Object();
            }
        }

        /// <summary>
        /// 内容序列化到字节数组
        /// </summary>
        /// <returns></returns>
        protected override byte[] OnSerializeToBytes() {
            return this.RootNode.SerializeToBytes();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            if (!this.RootNode.IsNull()) this.RootNode.Dispose();
            base.OnDispose();
        }

    }
}