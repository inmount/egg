using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 键/值存储集合
    /// </summary>
    public class KeyValues : egg.BasicObject, IDictionary<string, Values.Value> {

        private Dictionary<string, Values.Value> dict;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public KeyValues() {
            dict = new Dictionary<string, Values.Value>();
        }

        /// <summary>
        /// 返回默认值事件
        /// </summary>
        /// <returns></returns>
        protected virtual Values.Value OnReturnDefaultValue() { return new Values.None(); }

        /// <summary>
        /// 获取或设置键值内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Values.Value this[string key] {
            get {
                if (dict.ContainsKey(key)) {
                    if (eggs.Object.IsNull(dict[key])) return OnReturnDefaultValue();
                    return dict[key];
                } else {
                    return OnReturnDefaultValue();
                }
            }
            set {
                if (dict.ContainsKey(key)) {
                    // 释放原对象
                    dict[key].Dispose();
                    dict[key] = value;
                } else {
                    dict.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public ICollection<string> Keys { get { return dict.Keys; } }

        /// <summary>
        /// 获取值集合
        /// </summary>
        public ICollection<Values.Value> Values { get { return dict.Values; } }

        /// <summary>
        /// 获取集合内元素数量
        /// </summary>
        public int Count { get { return dict.Count; } }

        bool ICollection<KeyValuePair<string, Values.Value>>.IsReadOnly => throw new NotImplementedException();

        void IDictionary<string, Values.Value>.Add(string key, Values.Value value) {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, Values.Value>>.Add(KeyValuePair<string, Values.Value> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear() {
            dict.Clear();
        }

        bool ICollection<KeyValuePair<string, Values.Value>>.Contains(KeyValuePair<string, Values.Value> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取列表中是否存在键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            //throw new NotImplementedException();
            return dict.ContainsKey(key);
        }

        void ICollection<KeyValuePair<string, Values.Value>>.CopyTo(KeyValuePair<string, Values.Value>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取枚举管理器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, Values.Value>> GetEnumerator() {
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
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key) {
            return dict.Remove(key);
        }

        bool ICollection<KeyValuePair<string, Values.Value>>.Remove(KeyValuePair<string, Values.Value> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out Values.Value value) {
            return dict.TryGetValue(key, out value);
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
