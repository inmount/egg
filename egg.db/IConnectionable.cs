using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db {

    /// <summary>
    /// 数据库连接管理器接口
    /// </summary>
    public interface IConnectionable {

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        void Open(string connectionString);

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        void Close();

        /// <summary>
        /// 数据库打开状态
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Rows GetRows(string sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="sql">Generic Database Manipulation Language</param>
        /// <returns></returns>
        Row GetRow(string sql);

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql">Generic Database Manipulation Language</param>
        int Exec(string sql);

    }
}
