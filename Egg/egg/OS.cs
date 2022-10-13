using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace egg {

    /// <summary>
    /// 操作系统相关操作
    /// </summary>
    public static class OS {

        // 私有变量
        private static bool _isCheck = false;
        private static bool _isWindows = false;
        private static bool _isLinux = false;
        private static bool _isMacOS = false;

        private static void Check() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) _isWindows = true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) _isLinux = true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) _isMacOS = true;
            _isCheck = true;
        }

        /// <summary>
        /// 获取是否为Windows操作系统
        /// </summary>
        public static bool IsWindows {
            get {
                if (!_isCheck) Check();
                return _isWindows;
            }
        }

        /// <summary>
        /// 获取是否为Linux操作系统
        /// </summary>
        public static bool IsLinux {
            get {
                if (!_isCheck) Check();
                return _isLinux;
            }
        }

        /// <summary>
        /// 获取是否为MacOS操作系统
        /// </summary>
        public static bool IsMacOS {
            get {
                if (!_isCheck) Check();
                return _isMacOS;
            }
        }

    }
}
