using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 一个关系型数据库管理系统，由瑞典MySQL AB 公司开发，目前属于 Oracle 旗下产品
    /// </summary>
    public class SqliteConnection : egg.Object, IConnectionable {

        //数据库的连接管理器
        private Microsoft.Data.Sqlite.SqliteConnection dbc = null;

        /// <summary>
        /// 数据库是否开启
        /// </summary>
        public bool IsOpen {
            get {
                if (dbc == null) return false;
                return dbc.State == System.Data.ConnectionState.Open;
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        public void Open(string connectionString) {
            //throw new NotImplementedException();
            this.Close();
            dbc = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            dbc.Open();
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void Close() {
            if (this.IsOpen) dbc.Close();
            if (dbc != null) {
                dbc.Dispose();
                dbc = null;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 执行标准的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Exec(string sql) {
            //throw new NotImplementedException();
            int res = -1;
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, dbc)) {
                res = sqlCommand.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        /// 执行标准的SQL语句并返回列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Rows GetRows(string sql) {
            //throw new NotImplementedException();
            Rows ls = new Rows();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, dbc)) {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default)) {

                    while (reader.Read()) {
                        Row row = new Row();
                        for (int i = 0; i < reader.FieldCount; i++) {
                            //item.Set(SqlReader.GetName(i), SqlReader[i].ToString());
                            string szName = reader.GetName(i);
                            row[szName] = reader[i].ToString();
                        }
                        ls.Add(row);
                    }

                    reader.Close();
                }
            }
            return ls;
        }

        /// <summary>
        /// 执行标准的SQL语句并返回单行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Row GetRow(string sql) {
            //throw new NotImplementedException();
            Row row = new Row();
            using (Microsoft.Data.Sqlite.SqliteCommand sqlCommand = new Microsoft.Data.Sqlite.SqliteCommand(sql, dbc)) {
                using (Microsoft.Data.Sqlite.SqliteDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default)) {

                    if (reader.Read()) {
                        for (int i = 0; i < reader.FieldCount; i++) {
                            //item.Set(SqlReader.GetName(i), SqlReader[i].ToString());
                            string szName = reader.GetName(i);
                            row[szName] = reader[i].ToString();
                        }
                    }

                    reader.Close();
                }
            }
            return row;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            this.Close();
        }
    }
}
