using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Config {

    /// <summary>
    /// Linux系统常见配置类型
    /// </summary>
    public class Document : Serializable.Document {

        /// <summary>
        /// 获取全部的配置组
        /// </summary>
        public List<SettingGroup> Groups { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Document() {
            this.Groups = new List<SettingGroup>();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="str"></param>
        public Document(string str) {
            this.Groups = new List<SettingGroup>();
            Deserialize(str);
        }

        /// <summary>
        /// 获取一个设置组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SettingGroup this[string name] {
            get {
                for (int i = 0; i < this.Groups.Count; i++) {
                    SettingGroup group = this.Groups[i];
                    if (group.Name == name) return group;
                }
                SettingGroup newGroup = new SettingGroup() { Name = name };
                this.Groups.Add(newGroup);
                return newGroup;
            }
        }

        /// <summary>
        /// 移除配置组
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name) {
            for (int i = 0; i < this.Groups.Count; i++) {
                SettingGroup group = this.Groups[i];
                if (group.Name == name) {
                    this.Groups.RemoveAt(i);
                    group.Dispose();
                    return;
                }
            }
        }

        /// <summary>
        /// 清除所有配置项
        /// </summary>
        public void Clear() {
            for (int i = this.Groups.Count - 1; i >= 0; i--) {
                var group = this.Groups[i];
                this.Groups.RemoveAt(i);
                group.Dispose();
            }
            this.Groups.Clear();
        }

        // 反序列化内容填充
        protected override void OnDeserialize(Span<byte> bytes) {
            Deserialize(System.Text.Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// 反序列化内容填充
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Deserialize(string str) {
            // 清除所有配置组
            this.Clear();
            // 进行内容行化处理
            string[] lines = str.Replace("\r", "").Split('\n');
            SettingGroup group = null;

            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i].Trim();
                if (line.IsEmpty()) {
                    if (group == null) {
                        group = new SettingGroup() { Name = "" };
                        this.Groups.Add(group);
                    }
                } else if (line.StartsWith("[") && line.EndsWith("]")) {
                    group = new SettingGroup() { Name = line.Substring(1, line.Length - 2) };
                    this.Groups.Add(group);
                } else {
                    if (group == null) {
                        group = new SettingGroup() { Name = "" };
                        this.Groups.Add(group);
                    }
                    if (line.StartsWith("#")) {
                        group.AddNote(line.Substring(1));
                    } else {
                        int idx = line.IndexOf("=");
                        if (idx < 0) {
                            group.Set(line);
                        } else {
                            group.Set(line.Substring(0, idx).Trim(), line.Substring(idx + 1).Trim());
                        }
                    }
                }
            }
        }

        // 内容序列化到字节数组
        protected override byte[] OnSerializeToBytes() {
            return System.Text.Encoding.UTF8.GetBytes(SerializeToString());
        }

        /// <summary>
        /// 内容序列化到字符串
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string SerializeToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var g in this.Groups) {
                sb.Append(g.ToString());
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return SerializeToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Clear();
            this.Groups = null;
            base.OnDispose();
        }
    }
}
