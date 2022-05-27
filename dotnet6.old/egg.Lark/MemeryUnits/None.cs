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

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        protected override bool OnGetBoolean() { return false; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() { return 0; }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() { return null; }

    }
}
