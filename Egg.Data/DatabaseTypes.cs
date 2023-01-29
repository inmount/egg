using System;

namespace Egg.Data
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseTypes : int
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 0x0000,

        /// <summary>
        /// Oracle Database，又名Oracle RDBMS，简称Oracle。
        /// 甲骨文公司的一款关系数据库管理系统,
        /// </summary>
        Oracle = 0x0101,

        /// <summary>
        /// 一个关系型数据库管理系统，由瑞典MySQL AB 公司开发，目前属于 Oracle 旗下产品。
        /// </summary>
        MySQL = 0x0201,

        /// <summary>
        /// Microsoft 公司推出的关系型数据库管理系统。
        /// </summary>
        MicrosoftSqlServer = 0x0301,

        /// <summary>
        /// 加州大学伯克利分校计算机系开发的 POSTGRES，现在已经更名为PostgreSQL
        /// </summary>
        PostgreSQL = 0x0401,

        /// <summary>
        /// IBM DB2
        /// 美国IBM公司开发的一套关系型数据库管理系统
        /// </summary>
        DB2 = 0x0501,

        /// <summary>
        /// 由微软发布的关系数据库管理系统
        /// 连接mdb专用
        /// </summary>
        MicrosoftOfficeAccess = 0x0601,

        /// <summary>
        /// 由微软发布的关系数据库管理系统
        /// 连接accdb专用
        /// </summary>
        MicrosoftOfficeAccessV12 = 0x0602,

        /// <summary>
        /// 一款轻型的数据库，是遵守ACID的关系型数据库管理系统
        /// </summary>
        Sqlite = 0x0701,

        /// <summary>
        /// 一款轻型的数据库，是遵守ACID的关系型数据库管理系统
        /// 第三代专用
        /// </summary>
        Sqlite3 = 0x0702,
    }
}
