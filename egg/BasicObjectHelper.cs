using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace egg {

    /// <summary>
    /// 基础对象扩展
    /// </summary>
    public static class BasicObjectHelper {

        /// <summary>
        /// 是否为空字符串
        /// </summary>
        public static bool IsNull(this BasicObject obj) {
            return eggs.IsNull(obj);
        }

        /// <summary>
        /// 释放对象，可兼容为空处理
        /// </summary>
        public static void Destroy(this BasicObject obj) {
            if (!obj.IsNull()) obj.Dispose();
        }

    }
}
