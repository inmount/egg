using Egg.Data;
using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据仓库
    /// </summary>
    public class Repository<TClass, TId> where TClass : class, IEntity<TId>
    {
        // 数据库连接
        private readonly DatabaseConnection _connection;
        // 更新器
        private Updater<TClass, TId> _updater;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public DatabaseConnection Connection { get { return _connection; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="connection"></param>
        public Repository(DatabaseConnection connection)
        {
            _connection = connection;
            _updater = new Updater<TClass, TId>(connection);
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
            //var entity = await DbSet.FindAsync(id);
            //DbSet.Remove(entity);
            await Task.CompletedTask;
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
            //await DbSet.AddAsync(entity);
            await Task.CompletedTask;
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
            //await DbSet.AddRangeAsync(entity);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public TClass GetRow(string sql)
        {
            return _connection.GetRow<TClass>(sql);
        }

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<TClass> GetRows(string sql)
        {
            return _connection.GetRows<TClass>(sql);
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<TClass> GetRowAsync(string sql)
        {
            return await _connection.GetRowAsync<TClass>(sql);
        }

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<List<TClass>> GetRowsAsync(string sql)
        {
            return _connection.GetRowsAsync<TClass>(sql);
        }

        /// <summary>
        /// 获取更新器
        /// </summary>
        /// <returns></returns>
        public Updater<TClass, TId> Update() => _updater.UseAll();

        /// <summary>
        /// 获取更新器
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Update(Expression<Func<TClass, object?>> selector) => _updater.Use(selector);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async Task UpdateAsync(TClass entity, Expression<Func<TClass, bool>> predicate, Expression<Func<TClass, object?>>? selector = null)
        {
            var updater = new Updater<TClass, TId>(_connection);
            if (selector is null)
            {
                updater.UseAll();
            }
            else
            {
                updater.Use(selector);
            }
            updater.Set(entity, predicate);
            await Task.CompletedTask;
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
            //Context.Entry(entity).State = EntityState.Modified;
            //await Task.CompletedTask;
            await Task.CompletedTask;
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
            return await GetRowAsync("");
            //return await DbSet.FindAsync(id);
        }
    }
}
