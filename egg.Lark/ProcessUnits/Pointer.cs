using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.ProcessUnits {

    /// <summary>
    /// 存储指针
    /// </summary>
    public class Pointer : Unit {

        /// <summary>
        /// 获取关联的存储器
        /// </summary>
        public MemeryUnits.Function Function { get; private set; }

        /// <summary>
        /// 获取索引
        /// </summary>
        public int IntPtr { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        public Pointer(MemeryUnits.Function fn, int pointer) : base(UnitTypes.Pointer) {
            this.Function = fn;
            this.IntPtr = pointer;
        }

        /// <summary>
        /// 获取一个空指针
        /// </summary>
        public static Pointer None { get { return new Pointer(null, 0); } }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        protected override MemeryUnits.Unit OnGetMemeryUnit() {
            // 处理空指针
            if (eggs.IsNull(this.Function.Memery)) return new MemeryUnits.None();
            return this.Function.Memery[this.IntPtr];
        }

    }
}
