using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class Boolean : Unit {

        /// <summary>
        /// 获取值
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="val"></param>
        public Boolean(ScriptMemeryPool pool, bool val) : base(pool, UnitTypes.Boolean) {
            this.Value = val;
        }

        ///// <summary>
        ///// 实例化对象
        ///// </summary>
        ///// <returns></returns>
        //public static Boolean Create(bool val) { return new Boolean(val); }

        ///// <summary>
        ///// 新建一个为真的对象
        ///// </summary>
        //public static Boolean True => new Boolean(true);

        ///// <summary>
        ///// 新建一个为假的对象
        ///// </summary>
        //public static Boolean False => new Boolean(false);

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        protected override bool OnGetBoolean() { return this.Value; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() { return this.Value ? 1 : 0; }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() { return this.Value ? "true" : "false"; }
    }
}
