using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Egg {

    /// <summary>
    /// 对象扩展类
    /// </summary>
    public static class ObjectExtensions {

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj) {
            return obj is null;
        }

        /// <summary>
        /// 获取不为空的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultObj"></param>
        /// <returns></returns>
        public static T ToNotNull<T>(this object obj, T defaultObj) {
            return (T)(obj ?? (defaultObj ?? new object()));
        }

        /// <summary>
        /// 获取不为空的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToNotNull<T>(this object obj) where T : new() {
            if (obj is null) return new T();
            return (T)obj;
        }

    }
}
