using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Values {
    /// <summary>
    /// 对象
    /// </summary>
    public class Object<T> : Value {

        /// <summary>
        /// 获取关联对象
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public Object(T value) {
            Value = value;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        protected override object OnParseObject() {
            return this.Value;
        }

        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckEmpty() {
            return eggs.Object.IsNull(this.Value);
        }

    }
}
