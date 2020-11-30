using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 数据表格
    /// </summary>
    public class Rows : egg.Object, IList<Row> {

        private List<Row> list;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Rows() {
            list = new List<Row>();
        }

        Row IList<Row>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 获取行数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Row this[int index] {
            get { return list[index]; }
        }

        int ICollection<Row>.Count => throw new NotImplementedException();

        /// <summary>
        /// 获取表格内数据行数量
        /// </summary>
        public int Count {
            get { return list.Count; }
        }

        bool ICollection<Row>.IsReadOnly => throw new NotImplementedException();

        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="item"></param>
        public void Add(Row item) {
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

        bool ICollection<Row>.Contains(Row item) {
            throw new NotImplementedException();
        }

        void ICollection<Row>.CopyTo(Row[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取Enumerator
        /// </summary>
        public IEnumerator<Row> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        int IList<Row>.IndexOf(Row item) {
            throw new NotImplementedException();
        }

        void IList<Row>.Insert(int index, Row item) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        public void Insert(int index, Row item) {
            list.Insert(index, item);
        }

        bool ICollection<Row>.Remove(Row item) {
            throw new NotImplementedException();
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
        /// 释放内部对象
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            try {
                for (int i = 0; i < list.Count; i++) {
                    list[i].Dispose();
                }
                list.Clear();
                list = null;
            } catch { }
        }

    }
}
