using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 空节点
    /// </summary>
    public class Null : ValueNode {

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public Null() : base(NodeTypes.Null) { }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected override string OnSerialize() {
            return "null";
        }

        /// <summary>
        /// 为空节点
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckNull() {
            return true;
        }
    }
}
