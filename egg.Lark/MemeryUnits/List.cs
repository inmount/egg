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
                } else {
                    for (int i = ls.Count; i < index; i++) ls.Add(new None());
                    ls.Add(value);
                }
            }
        }

        protected override void OnDispose() {
            base.OnDispose();
            for (int i = 0; i < ls.Count; i++) {
                ls[i].Dispose();
            }
            ls.Clear();
        }
    }
}
