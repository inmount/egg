using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Egg.Log.Loggers {

    /// <summary>
    /// 文件日志
    /// </summary>
    public class FileLogger : ILogable {

        // 路径
        private string _path;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        public FileLogger(string path)
        {
            _path = egg.IO.GetClosedPath(path);
        }

        /// <summary>
        /// 输出内容
        /// </summary>
        /// <param name="entity"></param>
        public void Log(LogEntity entity)
        {
            string content = $"*{egg.Time.Now.ToDateTimeString()}* [{entity.Level.ToString().ToUpper()}] ({entity.Event}) {entity.Message}";
            var t = egg.Time.Now;
            string path = egg.IO.GetClosedPath($"{_path}{t.Year}-{t.Month.ToString().PadLeft(2, '0')}");
            egg.IO.CreateFolder(path);
            string filePath = $"{path}{entity.Level.ToString().ToLower()}-{t.ToDateString()}.log";
            using (var f = egg.IO.OpenFile(filePath, System.IO.FileMode.OpenOrCreate)) {
                f.Position = f.Length;
                f.WriteUtf8Line(content);
            }
        }
    }
}
