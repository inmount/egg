﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// 数据仓库
    /// </summary>
    public class RepositoryBase<TClass, TId> : IRepositoryBase<TClass, TId> where TClass : class, IEntityBase<TId>
    {

        /// <summary>
        /// DB上下文
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// 实体集合
        /// </summary>
        public DbSet<TClass> DbSet { get; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="context"></param>
        public RepositoryBase(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<TClass>();
        }

        /// <summary>
        /// 删除数
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TId id)
        {
            DeleteAsync(id).Wait();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TId id)
        {
            var entity = await DbSet.FindAsync(id);
            DbSet.Remove(entity);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Insert(TClass entity)
        {
            InsertAsync(entity).Wait();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(TClass entity)
        {
            await DbSet.AddAsync(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// 添加数据列表
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void InsertList(IEnumerable<TClass> entity)
        {
            InsertListAsync(entity).Wait();
        }

        /// <summary>
        /// 添加数据列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertListAsync(IEnumerable<TClass> entity)
        {
            await DbSet.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// 获取查询器
        /// </summary>
        /// <returns></returns>
        public IQueryable<TClass> Query()
        {
            return DbSet;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(TClass entity)
        {
            UpdateAsync(entity).Wait();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(TClass entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TClass Get(TId id)
        {
            return GetAsync(id).Result;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TClass> GetAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }
    }
}
