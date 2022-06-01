using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 可进行标准SQL字符串转化接口
    /// </summary>
    public interface ISqlTableStringable {

        /// <summary>
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp">数据库定义对象</param>
        /// <returns></returns>
        string ToSqlTableString(egg.db.DatabaseTypes tp);

    }
}
