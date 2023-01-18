using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.Dbsets
{
    /// <summary>
    /// 标准仓库接口
    /// </summary>
    public interface ISqlRepository
    {

        /// <summary>
        /// 查询单行数据
        /// </summary>
        /// <returns></returns>
        Task<T?> GetOneAsync<T>(string sql) where T : class, new();

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync<T>(string sql) where T : new();

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <returns></returns>
        Task<int> ExcuteNonQueryAsync(string sql);

        /// <summary>
        /// 查询单行数据
        /// </summary>
        /// <returns></returns>
        T? GetOne<T>(string sql) where T : class, new();

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(string sql) where T : new();

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <returns></returns>
        int ExcuteNonQuery(string sql);


    }
}
