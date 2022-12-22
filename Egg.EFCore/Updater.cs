using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.EFCore
{
    /// <summary>
    /// 更新器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Updater<T> where T : class
    {
        /// <summary>
        /// 更新器
        /// </summary>
        public Updater()
        {
            var tp = typeof(T);
        }
    }
}
