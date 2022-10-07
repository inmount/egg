using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Configure {

    /// <summary>
    /// 配置集合接口
    /// </summary>
    public interface IConfigureCollection {

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetSection(string key, Type type);

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetSection<T>(string key) where T : new();

        /// <summary>
        /// 设置配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        void SetSection(string key, object obj);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetConfigure(string key);

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetConfigure(string key, object value);

    }
}
