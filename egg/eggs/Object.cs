using System;
using System.Collections.Generic;
using egg;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// 对象操作
    /// </summary>
    public static class Object {

        // 基础对象管理器
        private static BasicObjectsMnanger objects = null;

        /// <summary>
        /// 获取基础对象管理器
        /// </summary>    
        public static BasicObjectsMnanger Objects {
            get {
                if (IsNull(objects)) objects = new BasicObjectsMnanger();
                return objects;
            }
        }

        /// <summary>
        /// 创建一个支持的对象并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateObject<T>() where T : BasicObject {
            return eggs.Object.Objects.Create<T>();
        }

        /// <summary>
        /// 获取同一种类型的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetObjects<T>() where T : BasicObject {
            return eggs.Object.Objects.GetObjects<T>();
        }

        /// <summary>
        /// 获取成员对象
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static BasicObject GetObjectById(long boid) {
            return eggs.Object.Objects.GetObjectById(boid);
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(System.Object obj) {
            return Equals(obj, null);
        }

        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str) {
            double temp = 0;
            if (double.TryParse(str, out temp)) {
                return true;
            }
            return false;
        }

    }
}


