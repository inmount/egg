using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 可进行标准SQL字符串转化接口
    /// </summary>
    public interface ISqlAsable {

        /// <summary>
        /// 允许对象重命名
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        SqlUnits.As As(string name);

    }
}
