using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using Egg.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.Common;
using System.Diagnostics;
using System.Collections;
using System.Data.SqlTypes;
using Egg;
using Egg.Data.Extensions;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 更新器
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class Updater<TClass, TId> where TClass : IEntity<TId>
    {
        /// <summary>
        /// 获取类型名称
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// 获取表名
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// 获取架构名
        /// </summary>
        public string? SchemaName { get; }

        /// <summary>
        /// 定义属性集合
        /// </summary>
        public List<ColumnProperty> Properties { get; }

        /// <summary>
        /// 数据库连接器
        /// </summary>
        public DatabaseConnection Connection { get; }

        /// <summary>
        /// 更新器
        /// </summary>
        public Updater(DatabaseConnection connection)
        {
            this.Connection = connection;
            this.Properties = new List<ColumnProperty>();
            // 解析并添加所有的属性定义
            var tp = typeof(TClass);
            this.TypeName = tp.GetTopName();
            this.TableName = tp.Name;
            var table = tp.GetCustomAttribute<TableAttribute>();
            if (table != null)
            {
                if (!table.Name.IsNullOrWhiteSpace()) this.TableName = table.Name;
                this.SchemaName = table.Schema;
            }
            var pros = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < pros.Length; i++)
            {
                var pro = pros[i];
                // 添加列属性
                this.Properties.Add(new ColumnProperty(this.Connection.Provider, pro));
            }
        }

        /// <summary>
        /// 添加更新字段
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Use(Expression<Func<TClass, object?>> selector)
        {
            var body = selector.Body;
            if (body is NewExpression)
            {
                var bodyNew = (NewExpression)selector.Body;
                foreach (var member in bodyNew.Members)
                {
                    var pro = this.Properties.Where(d => d.VarName == member.Name).FirstOrDefault();
                    if (pro is null) throw new Exception($"数据对象中不存在'{member.Name}'字段");
                    pro.IsModified = true;
                }
                return this;
            }
            if (body is UnaryExpression)
            {
                var bodyUnary = (UnaryExpression)selector.Body;
                var operand = (MemberExpression)bodyUnary.Operand;
                var member = operand.Member;
                var pro = this.Properties.Where(d => d.VarName == member.Name).FirstOrDefault();
                if (pro is null) throw new Exception($"数据对象中不存在'{member.Name}'字段");
                pro.IsModified = true;
                return this;
            }

            throw new Exception($"不支持的表达式类型: {body}");
        }

        /// <summary>
        /// 更新所有字段
        /// </summary>
        /// <returns></returns>
        public Updater<TClass, TId> UseAll()
        {
            foreach (var pro in this.Properties) pro.IsModified = true;
            return this;
        }

        public string GetSqlString(TClass entity, Expression<Func<TClass, bool>> predicate)
        {
            // 数据库供应商
            var provider = Connection.Provider;
            StringBuilder sb = new StringBuilder();
            // 拼接表相关字符串
            sb.Append("UPDATE ");
            if (this.SchemaName.IsNullOrWhiteSpace())
            {
                sb.Append(provider.GetNameString(this.TableName));
            }
            else
            {
                sb.Append(this.SchemaName);
                sb.Append(".");
                sb.Append(provider.GetNameString(this.TableName));
            }
            sb.Append(" SET ");
            // 处理字段
            StringBuilder sbSet = new StringBuilder();
            foreach (var pro in this.Properties)
            {
                if (!pro.IsModified) continue;
                if (sbSet.Length > 0) sbSet.Append(',');
                sbSet.Append(provider.GetNameString(pro.ColumnName));
                sbSet.Append('=');
                sbSet.Append(pro.GetSqlValue(entity));
            }
            sb.Append(sbSet.ToString());
            // 处理条件
            var body = (BinaryExpression)predicate.Body;
            sb.Append(" WHERE ");
            // 获取表达式SQL
            using (SqlExpression sqlExpression = new SqlExpression(provider, this.Properties))
            {
                sb.Append(sqlExpression.GetSqlString(body));
            }
            sb.Append(";");
            return sb.ToString();
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task SetAsync(TClass entity, Expression<Func<TClass, bool>> predicate)
        {
            string sql = GetSqlString(entity, predicate);
            // 执行更新
            await this.Connection.ExecuteNonQueryAsync(sql);
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task SetAsync(TClass entity)
        {
            // 执行更新
            await SetAsync(entity, d => Equals(d.Id, entity.Id));
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity, Expression<Func<TClass, bool>> predicate)
        {
            string sql = GetSqlString(entity, predicate);
            // 执行更新
            this.Connection.ExecuteNonQuery(sql);
            return this;
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity)
        {
            return Set(entity, d => Equals(d.Id, entity.Id));
        }

    }
}
