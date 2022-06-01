using System;
using egg;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// 控制台相关函数集合
    /// </summary>
    public static class Console {
        /// <summary>
        /// 获取控制台参数集合
        /// 参数可/或-字符进行指定
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static egg.Console.Arguments GetConsoleArgs(string[] args) {
            return new egg.Console.Arguments(args);
        }

    }

}
