using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace egg {
    
    /// <summary>
    /// 动态对象
    /// </summary>
    internal class DpzDynamicMetaObject : System.Dynamic.DynamicMetaObject {

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        public DpzDynamicMetaObject(Expression expression, object value) : base(expression, BindingRestrictions.Empty, value) {
            //json = (Json)value;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value) {
            //System.Diagnostics.Debug.WriteLine("设置" + binder.Name + "的值为:" + value.Value);
            //return base.BindSetMember(binder, value);
            // Method to call in the containing class:
            string methodName = "SetProperty";

            // setup the binding restrictions.
            BindingRestrictions restrictions =
                BindingRestrictions.GetTypeRestriction(Expression, LimitType);

            // setup the parameters:
            Expression[] args = new Expression[2];
            // First parameter is the name of the property to Set
            args[0] = Expression.Constant(binder.Name);
            // Second parameter is the value
            args[1] = Expression.Convert(value.Expression, typeof(object));

            // Setup the 'this' reference
            Expression self = Expression.Convert(Expression, LimitType);

            // Setup the method call expression
            Expression methodCall = Expression.Call(self,
                typeof(Dynamic).GetMethod(methodName),
            args);

            // Create a meta object to invoke Set later:
            DynamicMetaObject setDictionaryEntry = new DynamicMetaObject(
            methodCall,
            restrictions);
            // return that dynamic object
            return setDictionaryEntry;

        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="binder"></param>
        /// <returns></returns>
        public override DynamicMetaObject BindGetMember(GetMemberBinder binder) {
            //return base.BindGetMember(binder);
            // Method call in the containing class:
            System.Diagnostics.Debug.WriteLine("获取" + binder.Name + "的值");
            string methodName = "GetProperty";
            // One parameter
            Expression[] parameters = new Expression[]{
                Expression.Constant(binder.Name)
            };
            DynamicMetaObject getDictionaryEntry = new DynamicMetaObject(
                Expression.Call(
                    Expression.Convert(Expression, LimitType),
                    typeof(Dynamic).GetMethod(methodName),
                    parameters),
                BindingRestrictions.GetTypeRestriction(Expression, LimitType)
            );
            return getDictionaryEntry;

        }

    }

    /// <summary>
    /// 动态类型
    /// </summary>
    public class Dynamic : KeyList<object>, IDynamicMetaObjectProvider {

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SetProperty(string key, object value) {
            base[key] = value;
            return value;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetProperty(string key) {
            object result = null;
            if (base.ContainsKey(key)) {
                result = base[key];
            }
            return result;
        }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter) {
            //throw new NotImplementedException();
            return new DpzDynamicMetaObject(parameter, this);
        }
    }
}
