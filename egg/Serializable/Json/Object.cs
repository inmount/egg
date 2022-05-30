using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 对象节点
    /// </summary>
    public class Object : ObjectNode {

        // 建立列表和管理器
        private egg.KeyList<Node> nodes;

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public Object() : base(NodeTypes.Object) {
            nodes = new KeyList<Node>();
        }

        // 获取键集合
        protected override ICollection<string> OnGetKeys() { return nodes.Keys; }

        // 获取一个子对象
        protected override Node OnGetKeyItem(string key, bool autoCreate) {
            if (nodes.ContainsKey(key)) return nodes[key];
            if (autoCreate) {
                Null nul = this.Objects.Create<Null>();
                nodes[key] = nul;
                return nul;
            } else {
                return null;
            }
        }

        // 设置一个子对象
        protected override void OnSetKeyItem(string key, Node node) {
            // 清除原来的子对象
            if (nodes.ContainsKey(key)) {
                Node n = nodes[key];
                nodes.Remove(key);
                this.Objects.RemoveAt(n.BoId);
            }
            // 添加新的对象
            this.Objects.Add(node);
            nodes[key] = node;
        }

        // 清理对象
        protected override void OnClear() {
            nodes.Clear();
            this.Objects.Clear();
        }

        // 释放资源
        protected override void OnDispose() {
            this.Clear();
            nodes.Dispose();
            base.OnDispose();
        }

        // 获取序列化内容
        protected override string OnSerialize() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var item in nodes) {
                if (sb.Length > 1) sb.Append(",");
                sb.AppendFormat("\"{0}\":{1}", item.Key, item.Value.SerializeToString());
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
