using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Config {

    /// <summary>
    /// 设置组
    /// </summary>
    public class SettingGroup : BasicObject {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        private List<Line> lines;
        private List<Setting> settings;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public SettingGroup() {
            lines = new List<Line>();
            settings = new List<Setting>();
        }

        /// <summary>
        /// 添加一条空行
        /// </summary>
        public void AddEmptyLine() {
            Line line = new Line();
            lines.Add(line);
        }

        /// <summary>
        /// 添加一条注释
        /// </summary>
        /// <param name="content"></param>
        public void AddNote(string content) {
            Note note = new Note() { Content = content };
            lines.Add(note);
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public List<string> Keys {
            get {
                List<string> list = new List<string>();
                for (int i = 0; i < settings.Count; i++) {
                    Setting setting = settings[i];
                    list.Add(setting.Key);
                }
                return list;
            }
        }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get {
                for (int i = 0; i < settings.Count; i++) {
                    Setting setting = settings[i];
                    if (setting.Key == key) {
                        return setting.Value;
                    }
                }
                return null;
            }
            set { Set(key, value); }
        }


        /// <summary>
        /// 判断是否存在键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            for (int i = 0; i < settings.Count; i++) {
                Setting setting = settings[i];
                if (setting.Key == key) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value = null) {
            // 进行字符串兼容
            if (value.GetLength() >= 2) {
                if (value.StartsWith("\"") && value.EndsWith("\"")) value = value.Substring(1, value.Length - 2);
            }
            for (int i = 0; i < settings.Count; i++) {
                Setting setting = settings[i];
                if (setting.Key == key) {
                    setting.Value = value;
                    return;
                }
            }
            Setting newSetting = new Setting() {
                Key = key,
                Value = value
            };
            lines.Add(newSetting);
            settings.Add(newSetting);
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param key="key">The name of the setting to delete.</param>
        public void Delete(string key) {
            for (int i = 0; i < lines.Count; i++) {
                Line line = lines[i];
                if (line.GetType().Name == "Setting") {
                    Setting setting = (Setting)line;
                    if (setting.Key == key) {
                        lines.RemoveAt(i);
                        break;
                    }
                }
            }
            for (int i = 0; i < settings.Count; i++) {
                Setting setting = settings[i];
                if (setting.Key == key) {
                    settings.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            string res = "";
            if (Name != "") res += $"[{Name}]\r\n";

            for (int i = 0; i < lines.Count; i++) {
                Line line = lines[i];
                res += $"{line.ToString()}\r\n";
            }

            return res;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            for (int i = 0; i < lines.Count; i++) {
                lines[i].Dispose();
            }

            lines.Clear();
            lines = null;

            base.OnDispose();
        }

    }
}
