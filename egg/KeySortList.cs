﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 自排序键/值存储字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KeySortList<T> : egg.BasicObject, IDictionary<string, T> {

        private Dictionary<string, T> dict;
        private List<string> sortKeys;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public KeySortList() {
            dict = new Dictionary<string, T>();
            sortKeys = new List<string>();
        }

        /// <summary>
        /// 返回默认值事件
        /// </summary>
        /// <returns></returns>
        protected virtual T OnReturnDefaultValue() { return default(T); }

        /// <summary>
        /// 获取或设置键值内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key] {
            get {
                if (dict.ContainsKey(key)) {
                    return dict[key];
                } else {
                    return OnReturnDefaultValue();
                }
            }
            set {
                if (dict.ContainsKey(key)) {
                    dict[key] = value;
                } else {

                    for (int i = 0; i < sortKeys.Count; i++) {
                        if (key.SortBefore(sortKeys[i])) {
                            // 插入键
                            sortKeys.Insert(i, key);
                            dict.Add(key, value);
                            return;
                        }
                    }

                    // 添加一个新的键和键值
                    sortKeys.Add(key);
                    dict.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public ICollection<string> Keys {
            get {
                return sortKeys;
            }
        }

        /// <summary>
        /// 获取值集合
        /// </summary>
        public ICollection<T> Values { get { return dict.Values; } }

        /// <summary>
        /// 获取集合内元素数量
        /// </summary>
        public int Count { get { return dict.Count; } }

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly => throw new NotImplementedException();

        void IDictionary<string, T>.Add(string key, T value) {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear() {
            dict.Clear();
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item) {
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

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取枚举管理器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() {
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

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out T value) {
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
