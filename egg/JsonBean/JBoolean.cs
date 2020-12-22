using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用数字
    /// </summary>
    public class JBoolean : IUnit {

        #region [=====接口实现====]

        private IUnit _parent;

        /// <summary>
        /// 设置父对象
        /// </summary>
        /// <param name="p"></param>
        public void SetParent(IUnit p) { _parent = p; }

        /// <summary>
        /// 获取父对象
        /// </summary>
        public IUnit GetParent() { return _parent; }

        /// <summary>
        /// 获取单元类型
        /// </summary>
        public UnitTypes GetUnitType() { return _null ? UnitTypes.None : UnitTypes.Boolean; }

        #endregion

        private bool value;
        private bool _null;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public JBoolean() {
            value = false;
            _null = true;
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="val"></param>
        public JBoolean(bool val) {
            value = val;
            _null = false;
        }

        /// <summary>
        /// 快速创建一个布尔型对象
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static JBoolean Create(bool val) { return new JBoolean(val); }

        /// <summary>
        /// 判断结果是否为真
        /// </summary>
        /// <returns></returns>
        public bool IsTrue() { return value; }

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            return value ? "true" : "false";
        }

        #region [=====重载运算符=====]

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="bol"></param>
        public static implicit operator JBoolean(bool bol) {
            return new JBoolean(bol);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="bol"></param>
        public static implicit operator bool(JBoolean bol) {
            return bol.IsTrue();
        }

        #endregion

    }
}
