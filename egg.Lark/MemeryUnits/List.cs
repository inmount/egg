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
    }
}
