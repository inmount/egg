using Egg.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.Dbsets
{
    /// <summary>
    /// 仓库接口
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TClass, TId> where TClass : IEntity<TId>
    {

        /// <summary>
        /// 创建查询
        /// </summary>
        /// <returns></returns>
        IQueryable<TClass> Query();

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <returns></returns>
        TClass Get(TId id);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <returns></returns>
        Task<TClass> GetAsync(TId id);

        /// <summary>
        /// 新增
        /// </summary>
        void Insert(TClass entity);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        Task InsertAsync(TClass entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entity"></param>
        void InsertList(IEnumerable<TClass> entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entity"></param>
        Task InsertListAsync(IEnumerable<TClass> entity);

        /// <summary>
        /// 修改
        /// </summary>
        void Update(TClass entity);

        /// <summary>
        /// 修改
        /// </summary>
        Task UpdateAsync(TClass entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(TId id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(TId id);

    }
}
