﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Egg.Data
{
    /// <summary>
    /// 数据库供应器
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// 获取安全的值字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetValueString(string value);

        /// <summary>
        /// 获取名称定义字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        string GetNameString(string name);

        /// <summary>
        /// 获取函数定义
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetFuncString(string name, object[]? args);

        /// <summary>
        /// 获取完整的表名称
        /// </summary>
        /// <returns></returns>
        string GetFullTableName<T>();

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        IDatabaseConnectionBase GetDatabaseConnection(string connectionString);

        /// <summary>
        /// 获取事务开始语句
        /// </summary>
        /// <returns></returns>
        string GetTransactionBeginString();

        /// <summary>
        /// 获取事务结束语句
        /// </summary>
        /// <returns></returns>
        string GetTransactionEndString();

        #region [=====自动化=====]

        /// <summary>
        /// 获取表字段判断语句
        /// </summary>
        /// <returns></returns>
        string GetColumnExistsSqlString<T>(PropertyInfo property);

        /// <summary>
        /// 获取列添加语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetColumnAddSqlString<T>(PropertyInfo column);

        /// <summary>
        /// 获取数据表创建语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string GetTableCreateSqlString<T>();

        /// <summary>
        /// 获取数据表检测语句
        /// </summary>
        /// <returns></returns>
        string GetTableExistsSqlString<T>();

        /// <summary>
        /// 获取数据库检测语句
        /// </summary>
        /// <returns></returns>
        string GetDatabaseExistsSqlString(string name);

        /// <summary>
        /// 获取数据库创建语句
        /// </summary>
        /// <returns></returns>
        string GetDatabaseCreateSqlString(string name);

        /// <summary>
        /// 获取数据库查询语句
        /// </summary>
        /// <returns></returns>
        string GetDatabasesQuerySqlString();

        /// <summary>
        /// 获取分库检测语句
        /// </summary>
        /// <returns></returns>
        string GetSchemaExistsSqlString(string name);

        /// <summary>
        /// 获取分库创建语句
        /// </summary>
        /// <returns></returns>
        string GetSchemaCreateSqlString(string name);

        /// <summary>
        /// 获取分库查询语句
        /// </summary>
        /// <returns></returns>
        string GetSchemasQuerySqlString(string dbName, string userName);

        /// <summary>
        /// 获取表查询语句
        /// </summary>
        /// <returns></returns>
        string GetTablesQuerySqlString(string dbName, string? schema = null);

        /// <summary>
        /// 获取表字段查询语句
        /// </summary>
        /// <returns></returns>
        string GetColumnsQuerySqlString(string dbName, string tabName);

        /// <summary>
        /// 获取表字段查询语句
        /// </summary>
        /// <returns></returns>
        string GetColumnsQuerySqlString(string dbName, string schema, string tabName);

        #endregion
    }
}
