using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class NativeObject : Unit {

        /// <summary>
        /// 获取值
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// 获取关键字
        /// </summary>
        public egg.Strings Keys { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="obj"></param>
        public NativeObject(ScriptMemeryPool pool, object obj) : base(pool, UnitTypes.NativeObject) {
            this.Object = obj;
        }
    }
}
