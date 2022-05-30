using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 值节点
    /// </summary>
    public abstract class ValueNode : Node {

        /// <summary>
        /// 实例化一个节点
        /// </summary>
        /// <param name="tp"></param>
        public ValueNode(NodeTypes tp) : base(tp) { }

    }
}
