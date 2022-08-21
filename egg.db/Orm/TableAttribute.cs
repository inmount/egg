using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// 定义为Orm列
    /// </summary>
    public class TableAttribute : Attribute {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public TableAttribute() {
            Name = null;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public TableAttribute(string name) {
            Name = name;
        }

    }
}
