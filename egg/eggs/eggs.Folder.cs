using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

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
    /// <param name=""></param>
    /// <returns></returns>
    public static bool CheckFolderExists(string path) { return System.IO.Directory.Exists(path); }

    /// <summary>
    /// 检车文件是否存在
    /// </summary>
    /// <param name=""></param>
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

}
