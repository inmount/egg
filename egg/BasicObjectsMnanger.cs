using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 基础对象管理器，用于快速访问对象及批量对象操作
    /// </summary>
    public class BasicObjectsMnanger : IDisposable, IEnumerable<BasicObject> {

        // 基础对象列表
        private List<BasicObject> objects;
        // 索引器
        private long indexor;

        /// <summary>
        /// 获取所有者
        /// </summary>
        public object Owner { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        public BasicObjectsMnanger(object owner = null) {
            objects = new List<BasicObject>();
            this.Owner = owner;
        }

        /// <summary>
        /// 在管理器中创建一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>() where T : BasicObject {
            BasicObject obj = System.Activator.CreateInstance<T>() as BasicObject;
            obj.BoId = ++indexor;
            obj.BoManager = this;
            objects.Add(obj);
            return (T)obj;
        }

        /// <summary>
        /// 在管理器中创建一个对象
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public BasicObject Create(Type tp) {
            BasicObject obj = (BasicObject)System.Activator.CreateInstance(tp);
            obj.BoId = ++indexor;
            obj.BoManager = this;
            objects.Add(obj);
            return obj;
        }

        /// <summary>
        /// 添加一个对像，并返回对象的标识
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Add(BasicObject obj) {
            obj.BoId = ++indexor;
            obj.BoManager = this;
            objects.Add(obj);
            return obj.BoId;
        }

        /// <summary>
        /// 获取同一种类型的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetObjects<T>() where T : BasicObject {
            List<T> list = new List<T>();
            Type tp = typeof(T);
            foreach (BasicObject obj in objects) {
                if (obj.GetType().Equals(tp)) list.Add((T)obj);
            }
            return list;
        }

        /// <summary>
        /// 获取成员对象
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public BasicObject GetObjectById(long boid) {
            foreach (BasicObject obj in objects) {
                if (obj.BoId == boid) return obj;
            }
            return null;
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="boid"></param>
        public void RemoveAt(long boid) {
            foreach (BasicObject obj in objects) {
                if (obj.BoId == boid) {
                    objects.Remove(obj);
                    obj.BoId = 0;
                    obj.BoManager = null;
                    return;
                }
            }
        }

        /// <summary>
        /// 摧毁一个对象
        /// </summary>
        /// <param name="boid"></param>
        public void DestroyAt(long boid) {
            foreach (BasicObject obj in objects) {
                if (obj.BoId == boid) {
                    objects.Remove(obj);
                    obj.BoId = 0;
                    obj.BoManager = null;
                    obj.Dispose();
                    return;
                }
            }
        }

        /// <summary>
        /// 获取成员数量
        /// </summary>
        public int Count { get { return objects.Count; } }

        /// <summary>
        /// 获取成员对象
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public BasicObject this[long boid] { get { return GetObjectById(boid); } }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose() {
            for (int i = objects.Count - 1; i >= 0; i--) {
                BasicObject obj = objects[i];
                objects.RemoveAt(i);
                obj.BoId = 0;
                obj.BoManager = null;
                obj.Dispose();
            }
            objects.Clear();
            objects = null;
        }

        /// <summary>
        /// 支持foreach
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BasicObject> GetEnumerator() {
            return objects.GetEnumerator();
        }

        /// <summary>
        /// 支持foreach
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return objects.GetEnumerator();
        }
    }
}
