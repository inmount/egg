using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// 数据表格
    /// </summary>
    public class Rows<T> where T : Row, IDisposable {

        private List<T> list;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Rows() {
            list = new List<T>();
        }

        /// <summary>
        /// 获取行数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] {
            get { return list[index]; }
        }

        /// <summary>
        /// 获取表格内数据行数量
        /// </summary>
        public int Count {
            get { return list.Count; }
        }

        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item) {
            list.Add(item);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear() {
            for (int i = 0; i < list.Count; i++) {
                list[i].Dispose();
            }
            list.Clear();
        }

        /// <summary>
        /// 获取Enumerator
        /// </summary>
        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        public void Insert(int index, T item) {
            list.Insert(index, item);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public void RemoveAt(int index) {
            list[index].Dispose();
            list.RemoveAt(index);
        }

        /// <summary>
        /// 获取对象内元素是否为空
        /// </summary>
        public bool IsEmpty { get { return list.Count <= 0; } }

        /// <summary>
        /// 转化为Json对象
        /// </summary>
        /// <returns></returns>
        public Serializable.Json.List ToJsonList() {
            Serializable.Json.List res = new Serializable.Json.List();
            for (int i = 0; i < Count; i++) {
                res.Object(i, this[i].ToJsonObject());
                //res.Add(this[i].ToJsonObject());
            }
            return res;
        }

    }
}
