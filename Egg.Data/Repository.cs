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
            if (_connection.IsOpened)
                _provider = connection.Provider;
            _updater = new Updater<TClass, TId>(connection);
        }

        #region [=====公共模块=====]

        // 获取值字符串
        private string GetValueString(object? value)
        {
            switch (value)
            {
                case null: return "NULL";
                case string sz: return _provider.GetValueString((string)value);
                case DateTime dt: return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff")}'";
                default: return Convert.ToString(value);
            }
        }

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
                sb.Append(_provider.GetNameString(property.GetColumnName()));
            }
            return sb.ToString();
        }

        // 获取插入语句字段
        private string GetInsertColumns<T>()
        {
            StringBuilder sb = new StringBuilder();
            // 获取所有字段
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.IsAutoIncrement()) continue;
                if (sb.Length > 0) sb.Append(',');
                sb.Append(_provider.GetNameString(property.GetColumnName()));
            }
            return sb.ToString();
        }

        // 获取插入语句值
        private string GetInsertValues(TClass entity)
        {
            StringBuilder sb = new StringBuilder();
            // 获取所有字段
            Type type = typeof(TClass);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.IsAutoIncrement()) continue;
                if (sb.Length > 0) sb.Append(',');
                sb.Append(GetValueString(property.GetValue(entity)));
            }
            return sb.ToString();
        }

        #endregion

        #region [=====插入数据=====]

        // 获取插入sql语句
        public string GetInsertSqlString(TClass entity)
        {
            string columns = GetInsertColumns<TClass>();
            string values = GetInsertValues(entity);
            string fullTableName = _provider.GetFullTableName<TClass>();
            return $"INSERT INTO {fullTableName} ({columns}) VALUES ({values});";
        }

        // 获取插入列表sql语句
        public string GetInsertListSqlString(IEnumerable<TClass> entities)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entity in entities)
            {
                sb.AppendLine(GetInsertSqlString(entity));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TClass entity)
            => _connection.ExecuteNonQuery(GetInsertSqlString(entity));

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(TClass entity)
            => await _connection.ExecuteNonQueryAsync(GetInsertSqlString(entity));

        /// <summary>
        /// 插入数据列表
        /// </summary>
        /// <param name="entities"></param>
        public void InsertList(IEnumerable<TClass> entities)
            => _connection.ExecuteNonQuery(GetInsertListSqlString(entities));

        /// <summary>
        /// 插入数据列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertListAsync(IEnumerable<TClass> entities)
            => await _connection.ExecuteNonQueryAsync(GetInsertListSqlString(entities));

        #endregion

        #region [=====删除数据=====]

        // 获取根据Id获取单个实例的sql语句
        private string GetDeleteByIdSqlString(TId id)
        {
            // 获取数据
            string fullTableName = _provider.GetFullTableName<TClass>();
            string columnId = _provider.GetNameString(typeof(TClass).GetProperty(nameof(GuidKeyEntity.Id)).GetColumnName());
            string columnIdValue = _provider.GetValueString(Convert.ToString(id));
            return $"DELETE FROM {fullTableName} WHERE {columnId} = {columnIdValue};";
        }

        // 获取删除数据的sql语句
        private string GetDeleteSqlString(Expression<Func<TClass, bool>> predicate)
        {
            // 获取数据
            string fullTableName = _provider.GetFullTableName<TClass>();
            string where;
            // 处理条件
            var body = (BinaryExpression)predicate.Body;
            // 获取表达式SQL
            using (var sqlExpression = new SqlExpression<TClass>(_provider))
            {
                where = sqlExpression.GetSqlString(body);
            }
            return $"DELETE FROM {fullTableName} WHERE {where};";
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate"></param>
        public void Delete(Expression<Func<TClass, bool>> predicate)
            => _connection.ExecuteNonQuery(GetDeleteSqlString(predicate));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<TClass, bool>> predicate)
            => await _connection.ExecuteNonQueryAsync(GetDeleteSqlString(predicate));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TId id)
            => _connection.ExecuteNonQuery(GetDeleteByIdSqlString(id));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TId id)
            => await _connection.ExecuteNonQueryAsync(GetDeleteByIdSqlString(id));

        #endregion

        #region [=====更新数据=====]

        /// <summary>
        /// 获取更新器
        /// </summary>
        /// <returns></returns>
        public Updater<TClass, TId> Update()
            => _updater.UseAll();

        /// <summary>
        /// 获取更新器
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Update(Expression<Func<TClass, object?>> selector)
            => _updater.Use(selector);

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
            await updater.SetAsync(entity, predicate);
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

        // 获取根据Id获取单个实例的sql语句
        private string GetSelectSqlString(Expression<Func<TClass, bool>>? predicate = null)
        {
            // 获取数据
            string fullTableName = _provider.GetFullTableName<TClass>();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append("1 = 1");
            if (predicate != null)
            {
                // 处理条件
                var body = (BinaryExpression)predicate.Body;
                // 获取表达式SQL
                using (var sqlExpression = new SqlExpression<TClass>(_provider))
                {
                    sbWhere.Append(" AND ");
                    sbWhere.Append(sqlExpression.GetSqlString(body));
                }
            }
            string columns = GetSelectColumns<TClass>();
            return $"SELECT {columns} FROM {fullTableName} WHERE {sbWhere.ToString()};";
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TClass? GetRow(Expression<Func<TClass, bool>>? predicate = null)
            => _connection.GetRow<TClass>(GetSelectSqlString(predicate));

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TClass> GetRows(Expression<Func<TClass, bool>>? predicate = null)
            => _connection.GetRows<TClass>(GetSelectSqlString(predicate));

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TClass?> GetRowAsync(Expression<Func<TClass, bool>>? predicate = null)
            => await _connection.GetRowAsync<TClass>(GetSelectSqlString(predicate));

        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TClass>> GetRowsAsync(Expression<Func<TClass, bool>>? predicate = null)
            => await _connection.GetRowsAsync<TClass>(GetSelectSqlString(predicate));

        // 获取根据Id获取单个实例的sql语句
        private string GetEntityByIdSqlString(TId id)
        {
            // 获取数据
            string fullTableName = _provider.GetFullTableName<TClass>();
            string columnId = _provider.GetNameString(typeof(TClass).GetProperty(nameof(GuidKeyEntity.Id)).GetColumnName());
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
           => _connection.GetRow<TClass>(GetEntityByIdSqlString(id));

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TClass?> GetAsync(TId id)
            => await _connection.GetRowAsync<TClass>(GetEntityByIdSqlString(id));

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
