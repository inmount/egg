using System;

namespace egg {

    /// <summary>
    /// egg组件基础对象
    /// 提供销毁、转字符串等快速操作
    /// </summary>
    public abstract class Basic : IDisposable {

        /// <summary>
        /// 继承函数，销毁函数
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// 继承函数，获取字符串表现形式
        /// </summary>
        protected virtual string OnParseString() { return base.ToString(); }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return OnParseString(); }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose() {
            this.OnDispose();
        }
    }
}
