using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits.Functions {

    /// <summary>
    /// 最大值函数
    /// </summary>
    public class Max : Basic, ISqlStringable, ISqlAsable {

        // 对象
        private ISqlStringable _object;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="obj"></param>
        public Max(ISqlStringable obj) : base("MAX") {
            _object = obj;
        }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            return $"{this.Name}({_object.ToSqlString(tp, multiTable)})";
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public As As(string name) { return new As(this, name); }
    }
}
