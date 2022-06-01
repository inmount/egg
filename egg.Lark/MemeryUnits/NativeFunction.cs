using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class NativeFunction : Unit {

        /// <summary>
        /// 获取值
        /// </summary>
        public ScriptEngine.Function Function { get; private set; }

        /// <summary>
        /// 获取关键字
        /// </summary>
        public egg.Strings Keys { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="fn"></param>
        /// <param name="keys"></param>
        public NativeFunction(ScriptMemeryPool pool, ScriptEngine.Function fn, egg.Strings keys = null) : base(pool, UnitTypes.NativeFunction) {
            this.Function = fn;
            this.Keys = keys;
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        public MemeryUnits.Unit Execute(ScriptEngine.FunctionArgs args = null) {
            return this.Function(this.MemeryPool, args);
        }
    }
}
