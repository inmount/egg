using egg.Serializable.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace egg.File {

    /// <summary>
    /// 序列化文档
    /// </summary>
    public class UTF8SerializableFile<T> : egg.BasicObject where T : egg.Serializable.Document, new() {

        /// <summary>
        /// 获取关联文档
        /// </summary>
        public T Document { get; private set; }

        private string path = "";

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The filename where the configuration file can be found.</param>
        public UTF8SerializableFile(string file = "") {
            this.Document = new T();
            path = file;
            if (!path.IsEmpty()) LoadFile();
        }

        //加载文件内容
        private void LoadFile() {
            Span<byte> bytes = egg.File.UTF8File.ReadAllBytes(path, false);
            this.Document.Deserialize(bytes);
        }

        /// <summary>
        /// Saves the configuration to a file
        /// </summary>
        /// <param name="filename">The filename for the saved configuration file.</param>
        public void Save(string filename = null) {
            if (filename == null) filename = path;
            if (filename == "") throw new Exception("未找到存储地址");
            egg.File.UTF8File.WriteAllBytes(filename, this.Document.SerializeToBytes());
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Document.Dispose();
            base.OnDispose();
        }
    }
}
