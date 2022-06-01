using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 列表
    /// </summary>
    public class Object : Unit {
        // 列表对象
        private egg.KeyValues<Unit> ls;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public Object(ScriptMemeryPool pool) : base(pool, UnitTypes.Object) {
            ls = new egg.KeyValues<Unit>();
        }

        ///// <summary>
        ///// 实例化对象
        ///// </summary>
        ///// <returns></returns>
        //public static Object Create() { return new Object(); }

        /// <summary>
        /// 获取键名称集合
        /// </summary>
        public egg.Strings Keys {
            get {
                egg.Strings strs = new Strings();
                foreach (var item in ls.Keys) {
                    strs.Add(item);
                }
                return strs;
            }
        }

        /// <summary>
        /// 获取是否包含键名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            return ls.ContainsKey(key);
        }

        /// <summary>
        /// 获取或设置存储单元
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Unit this[string key] {
            get {
                int idx = key.IndexOf('.');
                if (idx > 0) {
                    string name = key.Substring(0, idx);
                    string cName = key.Substring(idx + 1);
                    if (cName.IsEmpty()) throw new Exception($"不支持空名称属性");
                    if (!ls.ContainsKey(name)) throw new Exception($"不存在'{name}'对象");
                    MemeryUnits.Unit obj = ls[name];
                    if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{name}'并非对象");
                    return ((MemeryUnits.Object)obj)[cName];
                } else {
                    if (ls.ContainsKey(key)) return ls[key];
                    return this.MemeryPool.None;
                }
            }
            set {
                int idx = key.IndexOf('.');
                if (idx > 0) {
                    string name = key.Substring(0, idx);
                    string cName = key.Substring(idx + 1);
                    if (cName.IsEmpty()) throw new Exception($"不支持空名称属性");
                    if (!ls.ContainsKey(name)) throw new Exception($"不存在'{name}'对象");
                    MemeryUnits.Unit obj = ls[name];
                    if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{name}'并非对象");
                    ((MemeryUnits.Object)obj)[cName] = value;
                    //value.Parent = this;
                } else {
                    ls[key] = value;
                    //value.Parent = this;
                }
            }
        }

        #region [=====便捷操作=====]

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Obj(string key) {
            return (MemeryUnits.Object)this[key];
        }

        /// <summary>
        /// 设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Obj(string key, Object val) {
            this[key] = val;
            return this;
        }

        /// <summary>
        /// 获取数组子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List Array(string key) {
            return (MemeryUnits.List)this[key];
        }

        /// <summary>
        /// 设置数组子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Array(string key, List val) {
            this[key] = val;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Number Num(string key) {
            return (MemeryUnits.Number)this[key];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Num(string key, Number val) {
            this[key] = val;
            return this;
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Num(string key, double val) {
            this[key] = base.MemeryPool.CreateNumber(val, this.Handle).MemeryUnit;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String Str(string key) {
            return (MemeryUnits.String)this[key];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Str(string key, String val) {
            this[key] = val;
            return this;
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Str(string key, string val) {
            this[key] = base.MemeryPool.CreateString(val, this.Handle).MemeryUnit;
            return this;
        }

        /// <summary>
        /// 获取数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean Bool(string key) {
            return (MemeryUnits.Boolean)this[key];
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Bool(string key, Boolean val) {
            this[key] = val;
            return this;
        }

        /// <summary>
        /// 设置数值子元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object Bool(string key, bool val) {
            this[key] = base.MemeryPool.CreateBoolean(val, this.Handle).MemeryUnit;
            return this;
        }

        #endregion

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
            foreach (var item in ls) {
                item.Value.Dispose();
            }
            ls.Clear();
        }
    }
}
