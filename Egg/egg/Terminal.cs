using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace egg
{
    /// <summary>
    /// 模拟终端
    /// </summary>
    public static class Terminal
    {
        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        public static string Execute(ProcessStartInfo processStartInfo)
        {
            using (Egg.Terminal term = new Egg.Terminal())
            {
                return term.Execute(processStartInfo);
            }
        }

        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Execute(ProcessStartInfo processStartInfo, Encoding encoding)
        {
            using (Egg.Terminal term = new Egg.Terminal(egg.Assembly.WorkingDirectory, encoding))
            {
                return term.Execute(processStartInfo);
            }
        }

        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Execute(string filePath, string? args)
        {
            using (Egg.Terminal term = new Egg.Terminal())
            {
                return term.Execute(filePath, args);
            }
        }

        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Execute(string filePath, string? args, Encoding encoding)
        {
            using (Egg.Terminal term = new Egg.Terminal(egg.Assembly.WorkingDirectory, encoding))
            {
                return term.Execute(filePath, args);
            }
        }
    }
}
