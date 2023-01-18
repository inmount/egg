using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.CompilerServices;
using Egg.EFCore.Dbsets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.Common;

namespace Egg.EFCore
{
    /// <summary>
    /// 更新器
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class Updater<TClass, TId> where TClass : IEntity<TId>
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        public string Name { get; }

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
        public List<UpdaterProperty> Properties { get; }

        /// <summary>
        /// DB上下文
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// 更新器
        /// </summary>
        public Updater(DbContext context)
        {
            this.Context = context;
            this.Properties = new List<UpdaterProperty>();
            // 解析并添加所有的属性定义
            var tp = typeof(TClass);
            this.Name = tp.Name;
            this.TableName = tp.Name;
            var table = tp.GetCustomAttribute<TableAttribute>();
            if (table != null)
            {
                if (!table.Name.IsEmpty()) this.TableName = table.Name;
                this.SchemaName = table.Schema;
            }
            var pros = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < pros.Length; i++) this.Properties.Add(new UpdaterProperty(pros[i]));
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
                    var pro = this.Properties.Where(d => d.Name == member.Name).FirstOrDefault();
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
                var pro = this.Properties.Where(d => d.Name == member.Name).FirstOrDefault();
                if (pro is null) throw new Exception($"数据对象中不存在'{member.Name}'字段");
                pro.IsModified = true;
                return this;
            }

            throw new Exception($"不支持的表达式类型: {body}");
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity)
        {
            return this;
        }

        // 获取sql语句值
        private string GetSqlExpressionValue(Expression exp)
        {
            if (exp is BinaryExpression)
                return "(" + GetSqlExpression((BinaryExpression)exp) + ")";
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var member = (MemberExpression)exp;
                    return "\"" + member.Member.Name + "\"";
                case ExpressionType.Constant:
                    var constant = (ConstantExpression)exp;
                    if (constant.Value is null) return "null";
                    if (constant.Value is string) return UpdaterProperty.GetSafetySqlString((string)constant.Value);
                    return constant.Value.ToString();
                default:
                    throw new Exception($"SqlExpressionValue不支持的'{exp.NodeType}'节点类型");
            }
        }

        // 获取sql语句
        private string GetSqlExpression(BinaryExpression exp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetSqlExpressionValue(exp.Left));
            switch (exp.NodeType)
            {
                case ExpressionType.AndAlso:
                    sb.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                    sb.Append(" or ");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                default: throw new Exception($"SqlExpression不支持的'{exp.NodeType}'节点类型");
            }
            sb.Append(GetSqlExpressionValue(exp.Right));
            return sb.ToString();
        }

        /// <summary>
        /// 设置需要保存的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Updater<TClass, TId> Set(TClass entity, Expression<Func<TClass, bool>> predicate)
        {
            Console.WriteLine($"Body: {predicate.Body}");
            StringBuilder sb = new StringBuilder();
            sb.Append($"update {(this.SchemaName.IsEmpty() ? "" : this.SchemaName + ".")}\"{this.TableName}\" set ");
            // 处理字段
            StringBuilder sbSet = new StringBuilder();
            foreach (var pro in this.Properties)
            {
                if (!pro.IsModified) continue;
                if (sbSet.Length > 0) sbSet.Append(',');
                sbSet.Append('"');
                sbSet.Append(pro.ColumnName);
                sbSet.Append('"');
                sbSet.Append('=');
                sbSet.Append(pro.GetSqlValue(entity));
            }
            sb.Append(sbSet.ToString());
            // 处理条件
            var body = (BinaryExpression)predicate.Body;
            sb.Append(" where ");
            sb.Append(GetSqlExpression(body));
            sb.AppendLine(";");
            return this;
        }

    }
}
