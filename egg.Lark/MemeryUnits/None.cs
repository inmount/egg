using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 空对象
    /// </summary>
    public class None : Unit {
        /// <summary>
        /// 实例化对象
        /// </summary>
        public None(ScriptMemeryPool pool) : base(pool, UnitTypes.None) { }

        ///// <summary>
        ///// 创建一个新的空对象
        ///// </summary>
        ///// <returns></returns>
        //public static None Create() { return new None(); }
    }
}
