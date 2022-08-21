using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// Row帮助类
    /// </summary>
    public static class RowHelper {

        /// <summary>
        /// 获取表定义
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Table GetTableDefine(this Row row) {
            // 获取类型
            Type tp = row.GetType();
            return Table.CreateTableDefine(tp);
        }

    }
}
