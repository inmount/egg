using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 一款轻型的数据库，是遵守ACID的关系型数据库管理系统
    /// </summary>
    public class SqliteDatabase : egg.db.Database {

        /// <summary>
        /// 存储路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public SqliteDatabase() {
            base.Type = egg.db.DatabaseTypes.SQLite;
        }

        /// <summary>
        /// 创建连接管理器
        /// </summary>
        /// <returns></returns>
        protected override IConnectionable OnCreateConnection() {
            return new SqliteConnection();
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            //return "server=" + this.Address + ";user id=" + this.User + ";password=" + this.Password + ";database=" + this.Name + ";port=" + this.Port + ";pooling=false;Connect Timeout=600;";
            return $"data source={this.Path}";
        }

    }
}
