using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace eggs {

    /// <summary>
    /// 配置集合
    /// </summary>
    public static class Configure {

        private static egg.Configure.IConfigureCollection _configure;

        /// <summary>
        /// 使用并初始化配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Use<T>() where T : egg.Configure.IConfigureCollection, new() {
            _configure = new T();
            return (T)_configure;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetSection(string key, Type type) {
            return _configure.GetSection(key, type);
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T GetSection<T>(string key) where T : new() {
            return _configure.GetSection<T>(key);
        }

    }
}
