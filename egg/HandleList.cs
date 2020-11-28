using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 带句柄的值存储字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HandleList<T> : egg.Object, IDictionary<long, T> {

        private Dictionary<long, T> dict;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HandleList() {
            dict = new Dictionary<long, T>();
        }

        /// <summary>
        /// 返回默认值事件
        /// </summary>
        /// <returns></returns>
        protected virtual T OnReturnDefaultValue() { return default(T); }

        /// <summary>
        /// 获取或设置键值内容
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public T this[long handle] {
            get {
                if (dict.ContainsKey(handle)) {
                    return dict[handle];
                } else {
                    return OnReturnDefaultValue();
                }
            }
            set {
                if (dict.ContainsKey(handle)) {
                    dict[handle] = value;
                } else {
                    dict.Add(handle, value);
                }
            }
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public ICollection<long> Keys { get { return dict.Keys; } }

        /// <summary>
        /// 获取值集合
        /// </summary>
        public ICollection<T> Values { get { return dict.Values; } }

        /// <summary>
        /// 获取集合内元素数量
        /// </summary>
        public int Count { get { return dict.Count; } }

        bool ICollection<KeyValuePair<long, T>>.IsReadOnly => throw new NotImplementedException();

        void IDictionary<long, T>.Add(long idx, T value) {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<long, T>>.Add(KeyValuePair<long, T> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear() {
            dict.Clear();
        }

        bool ICollection<KeyValuePair<long, T>>.Contains(KeyValuePair<long, T> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取列表中是否存在键值
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool ContainsKey(long handle) {
            //throw new NotImplementedException();
            return dict.ContainsKey(handle);
        }

        void ICollection<KeyValuePair<long, T>>.CopyTo(KeyValuePair<long, T>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取枚举管理器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<long, T>> GetEnumerator() {
            //throw new NotImplementedException();
            return dict.GetEnumerator();
        }

        /// <summary>
        /// 获取枚举接口
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            //throw new NotImplementedException();
            return dict.GetEnumerator();
        }

        /// <summary>
        /// 移除一个元素
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool Remove(long handle) {
            return dict.Remove(handle);
        }

        bool ICollection<KeyValuePair<long, T>>.Remove(KeyValuePair<long, T> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(long handle, out T value) {
            return dict.TryGetValue(handle, out value);
        }

        /// <summary>
        /// 获取对象内元素是否为空
        /// </summary>
        public bool IsEmpty { get { return dict.Count <= 0; } }

        /// <summary>
        /// 释放内存
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            dict.Clear();
        }

    }
}
