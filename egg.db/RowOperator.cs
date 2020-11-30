using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 行数据操作器
    /// </summary>
    public abstract class RowOperator : egg.Object {

        /// <summary>
        /// 获取行数据对象
        /// </summary>
        protected Row Row { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="row"></param>
        public RowOperator(Row row) {
            this.Row = row;
        }

        /// <summary>
        /// 获取基础数据行对象
        /// </summary>
        /// <returns></returns>
        public Row GetRow() { return this.Row; }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected virtual void OnSetValue(string key, string value) { }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RowOperator Set(string key, string value) {
            this.OnSetValue(key, value);
            return this;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();

            this.Row = null;
        }

    }
}
