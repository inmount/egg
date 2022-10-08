using System;
using System.Collections.Generic;
using System.Text;

namespace Egg {

    /// <summary>
    /// 小写字母索引字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LowerKeyDictionary<T> : StringKeyDictionary<T> {

        /// <summary>
        /// 获取或设置键值内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new T this[string key] {
            get {
                key = key.ToLower();
                return base[key];
            }
            set {
                key = key.ToLower();
                base[key] = value;
            }
        }

        /// <summary>
        /// 获取列表中是否存在键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(string key) {
            //throw new NotImplementedException();
            key = key.ToLower();
            return base.ContainsKey(key);
        }

        /// <summary>
        /// 移除一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool Remove(string key) {
            key = key.ToLower();
            return base.Remove(key);
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public new bool TryGetValue(string key, out T value) {
            key = key.ToLower();
            return base.TryGetValue(key, out value);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="NotImplementedException"></exception>
        public new void Add(string key, T value) {
            key = key.ToLower();
            base.Add(key, value);
        }

        /// <summary>
        /// 从字典中复制内容
        /// </summary>
        /// <param name="pairs"></param>
        public new void MapFrom(Dictionary<string, T> pairs) {
            foreach (var item in pairs) {
                this[item.Key] = item.Value;
            }
        }

    }
}
