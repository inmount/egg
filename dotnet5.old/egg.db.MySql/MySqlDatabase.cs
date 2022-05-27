using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 一个关系型数据库管理系统，由瑞典MySQL AB 公司开发，目前属于 Oracle 旗下产品
    /// </summary>
    public class MySqlDatabase : egg.db.Database {

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
        public MySqlDatabase() {
            base.Type = egg.db.DatabaseTypes.MySQL;
            this.Port = 3306;
        }

        /// <summary>
        /// 创建连接管理器
        /// </summary>
        /// <returns></returns>
        protected override IConnectionable OnCreateConnection() {
            return new MySqlConnection();
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return "server=" + this.Address + ";user id=" + this.User + ";password=" + this.Password + ";database=" + this.Name + ";port=" + this.Port + ";pooling=false;Connect Timeout=600;";
            return "server=" + this.Address + ";uid=" + this.User + ";pwd=" + this.Password + ";database=" + this.Name + ";port=" + this.Port + ";pooling=false;Connect Timeout=600;charset='utf8'";
        }

    }
}
