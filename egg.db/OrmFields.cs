using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// Orm框架的字段定义集合基类
    /// </summary>
    public abstract class OrmFields : egg.Object {

        /// <summary>
        /// 获取相关的字段定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual SqlUnits.TableField OnGetField(string name) { throw new Exception("未找到字段定义"); }

        /// <summary>
        /// 获取相关的字段定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SqlUnits.TableField this[string name] {
            get {
                return OnGetField(name);
            }
        }

    }
}
