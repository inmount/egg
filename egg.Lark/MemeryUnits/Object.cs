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
        public Object() : base(UnitTypes.Object) {
            ls = new egg.KeyValues<Unit>();
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
                    return new None();
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
                } else {
                    ls[key] = value;
                }
            }
        }

        protected override void OnDispose() {
            base.OnDispose();
            foreach (var item in ls) {
                item.Value.Dispose();
            }
            ls.Clear();
        }
    }
}
