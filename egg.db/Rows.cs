using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 数据表格
    /// </summary>
    public class Rows : List<Row>, IDisposable {

        /// <summary>
        /// 获取对象内元素是否为空
        /// </summary>
        public bool IsEmpty { get { return base.Count <= 0; } }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            this.Clear();
        }

        /// <summary>
        /// 转化为Json对象
        /// </summary>
        /// <returns></returns>
        public Serializable.Json.List ToJsonList() {
            Serializable.Json.List res = new Serializable.Json.List();
            for (int i = 0; i < this.Count; i++) {
                res.Object(i, this[i].ToJsonObject());
                //res.Add(this[i].ToJsonObject());
            }
            return res;
        }

    }
}
