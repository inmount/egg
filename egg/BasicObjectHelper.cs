using System;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// BasicObject助手类
    /// </summary>
    public static class BasicObjectHelper {

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this BasicObject obj) {
            return eggs.Object.IsNull(obj);
        }

        /// <summary>
        /// 能兼容为空情况的释放资源
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void Destroy(this BasicObject obj) {
            if (eggs.Object.IsNull(obj)) return;
            obj.BoId = 0;
            obj.BoManager = null;
            obj.Dispose();
        }

    }
}
