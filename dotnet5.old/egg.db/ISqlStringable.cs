using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 可进行标准SQL字符串转化接口
    /// </summary>
    public interface ISqlStringable {

        /// <summary>
        /// 获取或设置是否为复杂对象
        /// </summary>
        bool IsComplicated { get; set; }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp">数据库定义对象</param>
        /// <param name="multiTable">是否为多对象模式</param>
        /// <returns></returns>
        string ToSqlString(egg.db.DatabaseTypes tp, bool multiTable = false);

    }
}
