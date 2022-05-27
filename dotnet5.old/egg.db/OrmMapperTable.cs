using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// ORM数据快速映射器生成的表操作
    /// </summary>
    public class OrmMapperTable : SqlUnits.Table {

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public OrmMapperTable(string name) : base(name) { }

        /// <summary>
        /// 获取相关的字段定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SqlUnits.TableField this[string name] {
            get {
                return new SqlUnits.TableField(this, name);
            }
        }

    }
}
