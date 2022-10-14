using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// 数据库生成器
    /// </summary>
    public interface IDbCreater
    {
        /// <summary>
        /// 数据库确保创建
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> EnsureCreated(DbContext context);

        /// <summary>
        /// 获取数据库确保创建语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetEnsureCreatedSql(DbContext context);

        /// <summary>
        /// 获取数据表创建语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetCreateTableSql(IEntityType table);

        /// <summary>
        /// 获取数据列描述语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetColumnSql(IEntityType table, IMutableProperty column);

        /// <summary>
        /// 获取数据列创建语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetAddColumnSql(IEntityType table, IMutableProperty column);

        /// <summary>
        /// 获取数据列修改语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetAlterColumnSql(IEntityType table, IMutableProperty column);

    }
}
