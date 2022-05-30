using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 输入输出相关函数
    /// </summary>
    public static class IO {
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolder(string path) {
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 检车文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckFolderExists(string path) { return System.IO.Directory.Exists(path); }

        /// <summary>
        /// 检车文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckFileExists(string path) { return System.IO.File.Exists(path); }

        /// <summary>
        /// 获取所属子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string path) {
            return System.IO.Directory.GetDirectories(path);
        }

        /// <summary>
        /// 获取所属文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string pattern = null) {
            if (pattern == null) {
                return System.IO.Directory.GetFiles(path);
            } else {
                return System.IO.Directory.GetFiles(path, pattern);
            }
        }

        /// <summary>
        /// 获取Utf8文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static string GetUtf8FileContent(string path, bool create = false) {
            return egg.File.UTF8File.ReadAllText(path, create);
        }

        /// <summary>
        /// 设置Utf8文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static void SetUtf8FileContent(string path, string content) {
            egg.File.UTF8File.WriteAllText(path, content);
        }

        /// <summary>
        /// 打开并返回一个配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static egg.File.SerializableFile<egg.Serializable.Config.Document> OpenConfigDocument(string path) {
            return new egg.File.SerializableFile<egg.Serializable.Config.Document>(path);
        }

        /// <summary>
        /// 打开并返回一个Json文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static egg.File.SerializableFile<egg.Serializable.Json.Document> OpenJsonDocument(string path) {
            return new egg.File.SerializableFile<egg.Serializable.Json.Document>(path);
        }

        /// <summary>
        /// 打开并返回一个Xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static egg.File.SerializableFile<egg.Serializable.Xml.XmlDocument> OpenXmlDocument(string path) {
            return new egg.File.SerializableFile<egg.Serializable.Xml.XmlDocument>(path);
        }

        /// <summary>
        /// 打开并返回一个Html文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static egg.File.SerializableFile<egg.Serializable.Html.HtmlDocument> OpenHtmlDocument(string path) {
            return new egg.File.SerializableFile<egg.Serializable.Html.HtmlDocument>(path);
        }

        /// <summary>
        /// 打开并返回一个Markdown文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static egg.File.SerializableFile<egg.Serializable.Markdown.Document> OpenMarkdownDocument(string path) {
            return new egg.File.SerializableFile<egg.Serializable.Markdown.Document>(path);
        }
    }

}
