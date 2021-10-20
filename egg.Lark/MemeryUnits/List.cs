using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 列表
    /// </summary>
    public class List : Unit {
        // 列表对象
        private System.Collections.Generic.List<Unit> ls;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public List() : base(UnitTypes.List) {
            ls = new List<Unit>();
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <returns></returns>
        public static List Create() { return new List(); }

        /// <summary>
        /// 获取列表数量
        /// </summary>
        public int Count { get { return ls.Count; } }

        /// <summary>
        /// 获取或设置存储单元
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Unit this[int index] {
            get {
                if (index >= ls.Count) return new None();
                return ls[index];
            }
            set {
                if (index < ls.Count) {
                    ls[index] = value;
                    value.Parent = this;
                } else {
                    for (int i = ls.Count; i < index; i++) ls.Add(new None());
                    ls.Add(value);
                    value.Parent = this;
                }
            }
        }

        #region [=====便捷操作=====]

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Object Obj(int idx) {
            return (MemeryUnits.Object)this[idx];
        }

        /// <summary>
        /// 设置子对象
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List Obj(int idx, Object val) {
            this[idx] = val;
            return this;
        }

        /// <summary>
        /// 获取数组子对象
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public List Array(int idx) {
            return (MemeryUnits.List)this[idx];
        }

        /// <summary>
        /// 设置数组子对象
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List Array(int idx, List val) {
            this[idx] = val;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Number Num(int idx) {
            return (MemeryUnits.Number)this[idx];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List Num(int idx, Number val) {
            this[idx] = val;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public String Str(int idx) {
            return (MemeryUnits.String)this[idx];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List Str(int idx, String val) {
            this[idx] = val;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Boolean Bool(int idx) {
            return (MemeryUnits.Boolean)this[idx];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List Bool(int key, Boolean val) {
            this[key] = val;
            return this;
        }

        #endregion

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="unit"></param>
        public void Add(Unit unit) {
            ls.Add(unit);
            unit.Parent = this;
        }

        /// <summary>
        /// 清空元素
        /// </summary>
        public void Clear() {
            ls.Clear();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            for (int i = 0; i < ls.Count; i++) {
                ls[i].Dispose();
            }
            ls.Clear();
        }
    }
}
