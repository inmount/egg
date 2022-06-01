using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 查询语句中的排序信息
    /// </summary>
    public class SelectOrder : egg.BasicObject, ISqlStringable {

        // 排序对象
        private ISqlStringable _object;

        // 排序信息
        private string _sort;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sort"></param>
        public SelectOrder(ISqlStringable obj, string sort) {
            _object = obj;
            _sort = sort;
        }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            return $"{_object.ToSqlString(tp, multiTable)} {_sort}";
        }
    }
}
