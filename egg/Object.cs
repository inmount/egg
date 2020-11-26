using System;

namespace egg {

    /// <summary>
    /// 专用操作对象基类
    /// </summary>
    public abstract class Object : IDisposable {

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
            //throw new NotImplementedException();
            this.OnDispose();
        }

    }

}
