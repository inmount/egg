using Egg.Data;
using Egg.Data.Entities;
using Egg.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据仓库
    /// </summary>
    public class Repository<TClass, TId> : IRepository<TClass, TId> where TClass : class, IEntity<TId>
    {
        // 数据库连接
        private readonly IDatabaseConnection _connection;
        private readonly IDatabaseProvider _provider;
        // 更新器
        private Updater<TClass, TId> _updater;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDatabaseConnection Connection { get { return _connection; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="connection"></param>
        public Repository(IDatabaseConnection connection)
        {
            _connection = connection;
            _provider = connection.Provider;
            _updater = new Updater<TClass, TId>(connection);
        }

        #region [=====插入数据=====]

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

        #endregion

        #region [=====删除数据=====]

        /// <summary>
        /// 删除数据
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

        #endregion

        #region [=====更新数据=====]

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

        #endregion

        #region [=====查询数据=====]

        // 获取查询语句字段
        private string GetSelectColumns<T>()
        {
            StringBuilder sb = new StringBuilder();
            // 获取所有字段
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (sb.Length > 0) sb.Append(',');
                sb.Append(_provider.GetNameString(property.GetColumnAttributeName()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public TClass? GetRow(string sql)
            => _connection.GetRow<TClass>(sql);

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<TClass> GetRows(string sql)
            => _connection.GetRows<TClass>(sql);

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<TClass> GetRowAsync(string sql)
            => await _connection.GetRowAsync<TClass>(sql);

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<List<TClass>> GetRowsAsync(string sql)
            => _connection.GetRowsAsync<TClass>(sql);

        // 获取根据Id获取单个实例的sql语句
        private string GetEntityByIdSqlString(TId id)
        {
            // 获取数据
            string fullTableName = _provider.GetFullTableName<TClass>();
            string columnId = _provider.GetNameString(typeof(TClass).GetProperty(nameof(GuidKeyEntity.Id)).GetColumnAttributeName());
            string columnIdValue = _provider.GetValueString(Convert.ToString(id));
            string columns = GetSelectColumns<TClass>();
            return $"SELECT {columns} FROM {fullTableName} WHERE {columnId} = {columnIdValue};";
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TClass? Get(TId id)
           => GetRow(GetEntityByIdSqlString(id));

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TClass> GetAsync(TId id)
            => await GetRowAsync(GetEntityByIdSqlString(id));

        #endregion
    }
}
