using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// ORM数据快速映射生成器
    /// </summary>
    public class OrmMapper : egg.Object {

        /// <summary>
        /// 快速映射一个表定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public OrmMapperTable this[string name] {
            get {
                return new OrmMapperTable(name);
            }
        }

        /// <summary>
        /// 创建一个表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OrmMapperTable Table(string name) {
            return new OrmMapperTable(name);
        }

    }
}
