using egg.Serializable.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace egg.File {

    /// <summary>
    /// Linux通用配置文件
    /// </summary>
    public class ConfigFile : egg.BasicObject {

        // 建立文档
        Serializable.Config.Document doc = null;

        private string path = "";

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The filename where the configuration file can be found.</param>
        public ConfigFile(string file = "") {
            doc = new Serializable.Config.Document();
            path = file;
            if (!path.IsEmpty()) LoadFile();
        }

        //加载文件内容
        private void LoadFile() {
            string sz = egg.File.UTF8File.ReadAllText(path, false);
            doc.Deserialize(sz);
        }

        /// <summary>
        /// 获取一个设置组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SettingGroup this[string name] { get { return doc[name]; } }

        /// <summary>
        /// Saves the configuration to a file
        /// </summary>
        /// <param name="filename">The filename for the saved configuration file.</param>
        public void Save(string filename = null) {
            if (filename == null) filename = path;
            if (filename == "") throw new Exception("未找到存储地址");
            egg.File.UTF8File.WriteAllText(filename, doc.SerializeToString());
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return doc.SerializeToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            doc.Dispose();
            base.OnDispose();
        }
    }
}
