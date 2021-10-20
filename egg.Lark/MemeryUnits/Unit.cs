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
        /// 布尔
        /// </summary>
        Boolean = 0x01,
        /// <summary>
        /// 数值
        /// </summary>
        Number = 0x02,
        /// <summary>
        /// 字符串
        /// </summary>
        String = 0x03,
        /// <summary>
        /// 列表
        /// </summary>
        List = 0x04,
        /// <summary>
        /// 对象
        /// </summary>
        Object = 0x05,
        /// <summary>
        /// 函数
        /// </summary>
        Function = 0x06,
        /// <summary>
        /// 原生函数
        /// </summary>
        NativeObject = 0x11,
        /// <summary>
        /// 原生函数
        /// </summary>
        NativeFunction = 0x12,
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
        /// 单元类型
        /// </summary>
        public Unit Parent { get; internal set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="unitType"></param>
        public Unit(UnitTypes unitType) {
            this.UnitType = unitType;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnGetBoolean() { throw new Exception($"存储单元类型'{this.UnitType.ToString()}'不支持转化为数值。"); }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        public bool ToBoolean() { return OnGetBoolean(); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected virtual double OnGetNumber() { throw new Exception($"存储单元类型'{this.UnitType.ToString()}'不支持转化为数值。"); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double ToNumber() { return OnGetNumber(); }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetString() { throw new Exception($"存储单元类型'{this.UnitType.ToString()}'不支持转化为字符串。"); }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return OnGetString(); }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            //throw new NotImplementedException();
            this.OnDispose();
        }
    }
}
