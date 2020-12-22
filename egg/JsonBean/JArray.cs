using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用数组类型
    /// </summary>
    public class JArray : List<IUnit>, IUnit {

        private bool _null;

        #region [=====接口实现====]

        private IUnit _parent;

        /// <summary>
        /// 获取父对象
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
        public UnitTypes GetUnitType() { return _null ? UnitTypes.None : UnitTypes.Array; }

        #endregion

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="bNull"></param>
        public JArray(bool bNull = false) {
            _null = bNull;
        }

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            //throw new NotImplementedException();
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < base.Count; i++) {
                if (sb.Length > 1) sb.Append(',');
                sb.Append(base[i].ToJson());
            }
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// 添加一个空对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public JArray Add(JNull item) {
            //item.SetParent(this);
            if (_null) throw new Exception("Array is null");
            item.SetParent(this);
            base.Add(item);
            return this;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public JArray Add(JString item) {
            //item.SetParent(this);
            if (_null) throw new Exception("Array is null");
            item.SetParent(this);
            base.Add(item);
            return this;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public JArray Add(JNumber item) {
            if (_null) throw new Exception("Array is null");
            item.SetParent(this);
            base.Add(item);
            return this;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public JArray Add(JBoolean item) {
            if (_null) throw new Exception("Array is null");
            item.SetParent(this);
            base.Add(item);
            return this;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new JArray Add(IUnit item) {
            if (_null) throw new Exception("Array is null");
            var tp = item.GetType();
            var parent = tp.GetMethod("SetParent");
            parent.Invoke(item, new object[] { this });
            base.Add(item);
            return this;
        }

        /// <summary>
        /// 将对象中的内容复制给另一个对象
        /// </summary>
        /// <param name="arr"></param>
        public void CloneTo(JArray arr) {
            if (_null) throw new Exception("Array is null");
            for (int i = 0; i < this.Count; i++) {
                var tp = this[i].GetUnitType();
                switch (tp) {
                    case (UnitTypes.Array):
                        if (arr.Count <= i) {
                            arr.Add(this[i]);
                        } else {
                            ((JArray)this[i]).CloneTo((JArray)arr[i]);
                        }
                        break;
                    case UnitTypes.Object:
                        if (arr.Count <= i) {
                            arr.Add(this[i]);
                        } else {
                            ((JObject)this[i]).CloneTo((JObject)arr[i]);
                        }
                        break;
                    default:
                        if (arr.Count <= i) {
                            arr.Add(this[i]);
                        } else {
                            arr[i] = this[i];
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 从另一个对象中复制数组
        /// </summary>
        /// <param name="arr"></param>
        public void CloneFrom(JArray arr) {
            arr.CloneTo(this);
            _null = false;
        }

        /// <summary>
        /// 从另一个对象中复制对象
        /// </summary>
        /// <param name="arr"></param>
        public void CloneFrom(Json.JsonArray arr) {
            for (int i = 0; i < arr.Count; i++) {
                var utp = arr.GetChildUintType(i);
                switch (utp) {
                    case Json.UnitType.Object:
                        if (this.Count <= i) this.Add(new JObject());
                        ((JObject)this[i]).CloneFrom(arr.Object(i));
                        break;
                    case Json.UnitType.Array:
                        if (this.Count <= i) this.Add(new JArray());
                        ((JArray)this[i]).CloneFrom(arr.Array(i));
                        break;
                    case Json.UnitType.Number: this[i] = JNumber.Create(arr.Number(i)); break;
                    case Json.UnitType.String: this[i] = JString.Create(arr.String(i)); break;
                    case Json.UnitType.Boolean: this[i] = JBoolean.Create(arr.Bool(i)); break;
                }
            }
        }
    }
}
