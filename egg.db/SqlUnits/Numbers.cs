using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 字符串集合单元
    /// </summary>
    public class Numbers : egg.Object, ISqlStringable {

        // 值
        private double[] _value;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public Numbers(double[] value) {
            _value = value;
        }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string res = "";
            for (int i = 0; i < _value.Length; i++) {
                if (res != "") res += ",";
                res += $"{_value[i]}";
            }
            return $"({res})";
        }
    }
}
