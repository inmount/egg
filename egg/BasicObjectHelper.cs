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

    }
}
