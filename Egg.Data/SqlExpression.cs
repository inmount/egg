using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Reflection;
using Egg.Data.Extensions;

namespace Egg.Data
{
    /// <summary>
    /// Sql表达式
    /// </summary>
    public class SqlExpression<T> : IDisposable
    {
        // 数据库供应商
        private readonly IDatabaseProvider _provider;
        // 字段集合
        private readonly List<PropertyInfo> _properties;

        /// <summary>
        /// 数据库供应商
        /// </summary>
        public IDatabaseProvider Provider { get { return _provider; } }

        /// <summary>
        /// Sql表达式
        /// </summary>
        /// <param name="provider"></param>
        public SqlExpression(IDatabaseProvider provider)
        {
            _provider = provider;
            _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        }

        // 获取Contains函数兼容的sql语句
        private string GetContainsSql(MethodCallExpression call)
        {
            var callObj = (MemberExpression)call.Object;
            var callObjValues = GetSqlExpressionValue(callObj.Expression);
            if (callObjValues is null) throw new Exception($"容器无数据");
            var listInfo = callObjValues.GetType().GetField(callObj.Member.Name);
            ICollection list = (ICollection)listInfo.GetValue(callObjValues);
            StringBuilder sbList = new StringBuilder();
            sbList.Append('(');
            foreach (var item in list)
            {
                if (sbList.Length > 1) sbList.Append(", ");
                if (item is null) { sbList.Append("NULL"); continue; }
                if (item.GetType().IsNumeric()) { sbList.Append(item); continue; }
                if (item is string) { sbList.Append(_provider.GetValueString((string)item)); continue; }
                throw new Exception($"不支持的数据类型'{item.GetType().FullName}'");
            }
            sbList.Append(')');
            var arg = GetSqlExpressionValue(call.Arguments[0]);
            return $"{arg} IN {sbList.ToString()}";
        }

        // 获取Convert函数兼容的sql语句
        private string GetConvertSql(UnaryExpression unary)
        {
            object? value = null;
            if (unary.Operand is UnaryExpression) value = GetSqlExpressionValue(unary.Operand);
            if (unary.Operand is ConstantExpression) value = GetSqlExpressionValue(unary.Operand);
            if (value is null)
            {
                var operand = (MemberExpression)unary.Operand;
                var operandValues = GetSqlExpressionValue(operand.Expression);
                if (operandValues is null) throw new Exception($"容器无数据");
                var valueInfo = operandValues.GetType().GetField(operand.Member.Name);
                value = valueInfo.GetValue(operandValues);
            }
            switch (value)
            {
                case null: return "NULL";
                case string sz: return _provider.GetValueString((string)value);
                default: return Convert.ToString(value);
            }
        }

        // 获取sql语句值
        private object? GetSqlExpressionValue(Expression exp)
        {
            if (exp is BinaryExpression)
                return "(" + GetSqlString((BinaryExpression)exp) + ")";
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess: // 获取变量
                    var member = (MemberExpression)exp;
                    var pro = _properties.Where(d => d.Name == member.Member.Name).FirstOrDefault();
                    if (pro != null) return _provider.GetNameString(pro.GetColumnName());
                    return _provider.GetNameString(member.Member.Name);
                case ExpressionType.Constant: // 获取Constant函数
                    var constant = (ConstantExpression)exp;
                    if (constant.Value is null) return "NULL";
                    if (constant.Value is string) return _provider.GetValueString((string)constant.Value);
                    var valueType = constant.Value.GetType();
                    if (valueType.IsNumeric()) return constant.Value.ToString();
                    return constant.Value;
                case ExpressionType.Call: // 获取call函数
                    var call = (MethodCallExpression)exp;
                    var callMethod = call.Method;
                    if (callMethod.Name == "Contains") return GetContainsSql(call);
                    throw new Exception($"SqlExpressionValue不支持的Call类型'{callMethod.Name}'");
                case ExpressionType.Convert: // 获取Convert函数
                    return GetConvertSql((UnaryExpression)exp);
                default:
                    throw new Exception($"SqlExpressionValue不支持的'{exp.NodeType}'节点类型");
            }
        }

        /// <summary>
        /// 获取sql语句
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseException"></exception>
        public string GetSqlString(BinaryExpression exp)
        {
            StringBuilder sb = new StringBuilder();
            string expLeft = (string)(GetSqlExpressionValue(exp.Left) ?? "");
            string expRight = (string)(GetSqlExpressionValue(exp.Right) ?? "");
            sb.Append(expLeft);
            switch (exp.NodeType)
            {
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    if (expRight == "NULL")
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;
                case ExpressionType.NotEqual:
                    if (expRight == "NULL")
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
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
                case ExpressionType.Coalesce:
                    return $"COALESCE({expLeft}, {expRight})";
                default: throw new DatabaseException($"SqlExpression不支持的'{exp.NodeType}'节点类型");
            }
            sb.Append(expRight);
            return sb.ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
