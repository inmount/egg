using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 列表节点
    /// </summary>
    public class List : ObjectNode {

        // 建立列表和管理器
        private List<Node> nodes;

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public List() : base(NodeTypes.List) {
            nodes = new List<Node>();
        }

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(Node node) {
            // 添加新的对象
            this.Objects.Add(node);
            nodes.Add(node);
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            // 清理对象
            Node old = nodes[index];
            this.Objects.RemoveAt(old.BoId);
            // 删除节点
            nodes.RemoveAt(index);
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        protected override int OnGetItemCount() {
            return nodes.Count;
        }

        /// <summary>
        /// 获取一个子对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        protected override Node OnGetIndexItem(int index, bool autoCreate) {
            int count = nodes.Count;
            if (count > index) return nodes[index];
            if (autoCreate) {
                for (int i = count; i <= index; i++) {
                    Null nul = this.Objects.Create<Null>();
                    nodes.Add(nul);
                }
                return nodes[index];
            } else {
                return null;
            }
        }

        /// <summary>
        /// 设置一个子对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        protected override void OnSetIndexItem(int index, Node node) {
            int count = nodes.Count;
            if (count > index) {
                // 清理原来的对象再换成现在的对象
                Node old = nodes[index];
                this.Objects.Add(node);
                nodes[index] = node;
                this.Objects.RemoveAt(old.BoId);
                return;
            }
            // 填充空对象
            for (int i = count; i < index; i++) {
                Null nul = this.Objects.Create<Null>();
                nodes.Add(nul);
            }
            // 添加新的对象
            this.Objects.Add(node);
            nodes.Add(node);
        }

        /// <summary>
        /// 清理对象
        /// </summary>
        protected override void OnClear() {
            nodes.Clear();
            this.Objects.Clear();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Clear();
            nodes = null;
            base.OnDispose();
        }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected override string OnSerialize() {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in nodes) {
                if (sb.Length > 1) sb.Append(",");
                sb.Append(item.SerializeToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
