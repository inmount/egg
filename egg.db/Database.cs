using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseTypes {

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
        Microsoft_SQL_Server = 0x0301,

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
        Microsoft_Office_Access = 0x0601,

        /// <summary>
        /// 由微软发布的关系数据库管理系统
        /// 连接accdb专用
        /// </summary>
        Microsoft_Office_Access_v12 = 0x0602,

        /// <summary>
        /// 一款轻型的数据库，是遵守ACID的关系型数据库管理系统
        /// </summary>
        SQLite = 0x0701,

        /// <summary>
        /// 一款轻型的数据库，是遵守ACID的关系型数据库管理系统
        /// 第三代专用
        /// </summary>
        SQLite_3 = 0x0702,
    }

    /// <summary>
    /// 数据库定义操作静态类
    /// </summary>
    public abstract class Database : egg.Object {

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public DatabaseTypes Type { get; protected set; }

        // 检查有效性
        private static bool CheckEnable(string str) {
            if (str.IsNull()) return false;
            str = str.ToLower();
            if (str == "yes") return true;
            if (str == "no") return true;
            return false;
        }

        ///// <summary>
        ///// 从配置中创建事件
        ///// </summary>
        ///// <param name="setting"></param>
        ///// <returns></returns>
        //protected virtual egg.db.Database OnCreateByConfSetting(egg.File.Conf.SettingGroup setting) { return null; }

        ///// <summary>
        ///// 从配置文件中加载数据库定义
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="group"></param>
        ///// <returns></returns>
        //public static egg.db.Database LoadFromConf(string path, string group = "default") {

        //    // 当文件不存在时，执行初始化创建
        //    if (!System.IO.File.Exists(path)) {
        //        using (egg.File.ConfFile file = new File.ConfFile(path)) {

        //            // 建立Sqlserver示例
        //            var sqlserverGroup = file["SqlServer"];
        //            sqlserverGroup["Enable"] = "no";
        //            sqlserverGroup["Type"] = "SqlServer";
        //            sqlserverGroup["Address"] = "127.0.0.1";
        //            sqlserverGroup["Port"] = "1433";
        //            sqlserverGroup["Name"] = "master";
        //            sqlserverGroup["User"] = "sa";
        //            sqlserverGroup["Password"] = "123456";

        //            // 建立mysql示例
        //            var mysqlGroup = file["MySql"];
        //            mysqlGroup["Enable"] = "no";
        //            mysqlGroup["Type"] = "MySql";
        //            mysqlGroup["Address"] = "127.0.0.1";
        //            mysqlGroup["Port"] = "3306";
        //            mysqlGroup["Name"] = "mysql";
        //            mysqlGroup["User"] = "root";
        //            mysqlGroup["Password"] = "123456";

        //            // 建立mysql示例
        //            var pgsqlGroup = file["PostgreSql"];
        //            pgsqlGroup["Enable"] = "no";
        //            pgsqlGroup["Type"] = "PostgreSql";
        //            pgsqlGroup["Address"] = "127.0.0.1";
        //            pgsqlGroup["Port"] = "5432";
        //            pgsqlGroup["Name"] = "postgres";
        //            pgsqlGroup["User"] = "root";
        //            pgsqlGroup["Password"] = "123456";

        //            // 建立sqlite示例
        //            var sqliteGroup = file["Sqlite"];
        //            sqliteGroup["Enable"] = "no";
        //            sqliteGroup["Type"] = "Sqlite";
        //            sqliteGroup["Path"] = "/db/sqlite.db";

        //            //文件保存
        //            file.Save();
        //        }
        //    }

        //    using (egg.File.ConfFile file = new File.ConfFile(path)) {
        //        var confGroup = file[group];

        //        // 当节点不存在时，返回null
        //        if (confGroup == null) return null;
        //        if (!CheckEnable(confGroup["Enable"])) return null;

        //        string dbType = confGroup["Type"];
               

        //        switch (dbType.ToLower()) {
        //            case "sqlserver":
        //                return new Databases.MicrosoftSqlServer() {
        //                    Address = confGroup["Address"],
        //                    Port = confGroup["Port"].ToInteger(),
        //                    Name = confGroup["Name"],
        //                    User = confGroup["User"],
        //                    Password = confGroup["Password"]
        //                };
        //            case "mysql":
        //                return new Databases.MySql() {
        //                    Address = confGroup["Address"],
        //                    Port = confGroup["Port"].ToInteger(),
        //                    Name = confGroup["Name"],
        //                    User = confGroup["User"],
        //                    Password = confGroup["Password"]
        //                };
        //            case "postgresql":
        //            case "pgsql":
        //                return new Databases.PostgreSql() {
        //                    Address = confGroup["Address"],
        //                    Port = confGroup["Port"].ToInteger(),
        //                    Name = confGroup["Name"],
        //                    User = confGroup["User"],
        //                    Password = confGroup["Password"]
        //                };
        //            case "sqlite":
        //                return new Databases.Sqlite() {
        //                    Path = confGroup["Path"]
        //                };
        //            default:
        //                throw new Exception($"不支持的数据库类型:{dbType}");
        //        }

        //    }
        //}

        /// <summary>
        /// 创建连接事件
        /// </summary>
        /// <returns></returns>
        protected virtual IConnectionable OnCreateConnection() { return null; }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public IConnectionable CreateConnection() { return OnCreateConnection(); }

    }
}
