using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {

    /// <summary>
    /// 存储类型
    /// </summary>
    public enum UnitTypes {
        /// <summary>
        /// 空值
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 数值
        /// </summary>
        Number = 0x01,
        /// <summary>
        /// 字符串
        /// </summary>
        String = 0x02,
        /// <summary>
        /// 列表
        /// </summary>
        List = 0x03,
        /// <summary>
        /// 对象
        /// </summary>
        Object = 0x04,
        /// <summary>
        /// 函数
        /// </summary>
        Function = 0x05,
        /// <summary>
        /// 原生函数
        /// </summary>
        NativeFunction = 0x06,
    }

    /// <summary>
    /// 处理单元
    /// </summary>
    public abstract class Unit : IDisposable {
        /// <summary>
        /// 单元类型
        /// </summary>
        public UnitTypes UnitType { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="unitType"></param>
        public Unit(UnitTypes unitType) {
            this.UnitType = unitType;
        }

        // 释放资源
        protected virtual void OnDispose() {

        }

        // 释放资源
        public void Dispose() {
            //throw new NotImplementedException();
            this.OnDispose();
        }
    }
}
