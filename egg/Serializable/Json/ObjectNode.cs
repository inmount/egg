using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 对象节点
    /// </summary>
    public abstract class ObjectNode : Node {

        /// <summary>
        /// 获取Bo对象管理器
        /// </summary>
        protected BasicObjectsMnanger Objects { get; private set; }

        /// <summary>
        /// 实例化一个节点
        /// </summary>
        /// <param name="tp"></param>
        public ObjectNode(NodeTypes tp) : base(tp) {
            this.Objects = new BasicObjectsMnanger(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Objects.Dispose();
            base.OnDispose();
        }

    }
}
