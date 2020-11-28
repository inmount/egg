using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace egg {

    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger : egg.Object {

        // 操作队列
        private List<string> _list;

        // 任务线程
        private Thread _task;
        private bool _working;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        public Logger(string path) {
            path = path.Replace("\\", "/");
            if (!path.EndsWith("/")) path += "/";
            // 初始化操作队列
            _list = new List<string>();
            // 初始化任务线程
            _working = true;
            _task = new Thread(() => {
                // 当前文件名
                string fileNameCache = null;
                // 操作流
                FileStream stream = null;
                // 循环执行线程
                while (_working) {
                    // 清理列队
                    for (int i = _list.Count - 1; i >= 0; i--) {
                        if (_list[i] == null) _list.RemoveAt(i);
                    }
                    // 则将队列中的内容写入日志
                    for (int i = 0; i < _list.Count; i++) {
                        // 设置读写文件
                        string fileName = $"{eggs.GetNowString("yyyyMMdd")}.log";
                        string filePath = $"{path}{fileName}";
                        if (fileNameCache != fileName) {
                            // 释放当前操作流
                            if (stream != null) {
                                stream.Dispose();
                                stream = null;
                            }
                            fileNameCache = fileName;
                            // 创建文件
                            if (!System.IO.File.Exists(filePath)) {
                                using (var file = System.IO.File.Open(filePath, FileMode.Create)) {
                                    file.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                                }
                            }
                            //stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Write);
                        }
                        if (stream == null) stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.Read);
                        // 设置写入位置
                        stream.Position = stream.Length;
                        // 生成内容
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(_list[i]);
                        // 输出内容到文件中
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                        _list[i] = null;
                    }
                    Thread.Sleep(1);
                }
                if (stream != null) {
                    stream.Dispose();
                    stream=null;
                }
            });
            _task.Start();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content) {
            _list.Add(content);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            try {
                _working = false;
            } catch { }
            base.OnDispose();
        }

    }
}
