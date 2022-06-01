using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// Orm专用表定义
    /// </summary>
    public class OrmTable : SqlUnits.Table {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public OrmTable(string name) : base(name) { }

        /// <summary>
        /// 创建一个行数据操作器
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected virtual RowOperator OnRowerCreate(egg.db.Row row) { throw new Exception("尚未定义此操作"); }

        /// <summary>
        /// 创建一个行数据操作器
        /// </summary>
        /// <param name="row">行数据对象</param>
        public RowOperator Rower(egg.db.Row row = null) {
            if (row == null) row = new egg.db.Row();
            return OnRowerCreate(row);
        }

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
