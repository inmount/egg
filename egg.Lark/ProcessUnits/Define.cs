using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.ProcessUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class Define : Unit {

        /// <summary>
        /// 获取关联的存储器
        /// </summary>
        public MemeryUnits.Function Function { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="name"></param>
        public Define(MemeryUnits.Function fn, string name) : base(UnitTypes.Define) {
            this.Function = fn;
            this.Name = name;
        }

        protected override MemeryUnits.Unit OnGetMemeryUnit() {
            // 处理空指针
            if (eggs.IsNull(this.Function.Memery)) return new MemeryUnits.None();
            return this.Function.GetVarValue(this.Name);
        }
    }
}
