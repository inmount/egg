using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据仓库
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TClass, TId> where TClass : class, IEntity<TId>
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        IDatabaseConnection Connection { get; }

        #region [=====插入数据=====]

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">数据实例</param>
        void Insert(TClass entity);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">数据实例</param>
        /// <returns></returns>
        Task InsertAsync(TClass entity);

        /// <summary>
        /// 添加数据列表
        /// </summary>
        /// <param name="entities">数据实例集合</param>
        /// <exception cref="NotImplementedException"></exception>
        void InsertList(IEnumerable<TClass> entities);

        /// <summary>
        /// 添加数据列表
        /// </summary>
        /// <param name="entities">数据实例集合</param>
        /// <returns></returns>
        Task InsertListAsync(IEnumerable<TClass> entities);

        #endregion

        #region [=====删除数据=====]

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(TId id);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task DeleteAsync(TId id);

        #endregion

        #region [=====更新数据=====]

        #endregion

        #region [=====查询数据=====]

        #endregion
    }
}
