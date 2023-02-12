using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

namespace egg
{

    /// <summary>
    /// 程序集
    /// </summary>
    public static class Assembly
    {

        // 私有变量
        private static string? _executionFilePath = null;
        private static string? _executionDirectory = null;
        private static string? _workingDirectory = null;
        private static FileVersionInfo? _info = null;

        /// <summary>
        /// 获取执行程序信息
        /// </summary>
        public static FileVersionInfo ExecutionInfo
        {
            get
            {
                if (_info == null)
                {
                    _info = FileVersionInfo.GetVersionInfo(ExecutionFilePath);
                }
                return _info;
            }
        }

        /// <summary>
        /// 获取程序版本
        /// </summary>
        public static string Version { get { return ExecutionInfo.ProductVersion; } }

        /// <summary>
        /// 获取程序名称
        /// </summary>
        public static string Name { get { return ExecutionInfo.ProductName; } }

        /// <summary>
        /// 获取程序版本
        /// </summary>
        public static string ExecutionFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_executionFilePath))
                {
                    _executionFilePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                }
                return _executionFilePath;
            }
        }

        /// <summary>
        /// 获取程序目录
        /// </summary>
        public static string ExecutionDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_executionDirectory))
                {
                    _executionDirectory = IO.GetClosedPath(System.IO.Path.GetDirectoryName(ExecutionFilePath));
                }
                return _executionDirectory;
            }
        }

        /// <summary>
        /// 获取或设置工作目录
        /// </summary>
        public static string WorkingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_workingDirectory))
                {
                    _workingDirectory = IO.GetClosedPath(Environment.CurrentDirectory);
                }
                return _workingDirectory;
            }
            set { _workingDirectory = value; }
        }

        /// <summary>
        /// 获取程序参数集
        /// </summary>
        public static Egg.Arguments.IArguments? Arguments { get; private set; }

        /// <summary>s
        /// 设置参数集合
        /// </summary>
        /// <returns></returns>
        public static void SetArguments<T>(string[] args) where T : Egg.Arguments.IArguments, new()
        {
            Arguments = CreateArguments<T>(args);
        }

        /// <summary>
        /// 获取参数集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T CreateArguments<T>(string[] args) where T : Egg.Arguments.IArguments, new()
        {
            var obj = new T();
            obj.SetParams(args);
            return obj;
        }

        /// <summary>
        /// 根据名称查找类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type? FindType(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                    if (type.FullName == name) return type;
            }
            return null;
        }

    }
}
