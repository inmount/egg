using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// Microsoft 公司推出的关系型数据库管理系统
    /// </summary>
    public class MicrosoftSqlServerDatabase : egg.db.Database {

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MicrosoftSqlServerDatabase() {
            base.Type = egg.db.DatabaseTypes.Microsoft_SQL_Server;
            this.Port = 1433;
        }

        /// <summary>
        /// 创建连接管理器
        /// </summary>
        /// <returns></returns>
        protected override IConnectionable OnCreateConnection() {
            return new MicrosoftSqlServerConnection();
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return "data source=" + this.Address + "," + this.Port + ";user id=" + this.User + ";Password=" + this.Password + ";Initial Catalog=" + this.Name + ";Pooling=true;Max Pool Size=40000;Min Pool Size=0;";
        }

    }
}
