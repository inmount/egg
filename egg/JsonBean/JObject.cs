using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用对象
    /// </summary>
    public class JObject : Object, IUnit {

        // 子对象列表
        private KeyList<IUnit> list;
        private bool _null;
        private Type _type;

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
        public UnitTypes GetUnitType() { return _null ? UnitTypes.None : UnitTypes.Object; }

        #endregion

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JObject(Type tp) {
            _null = true;
            _type = tp;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JObject() {
            list = new KeyList<IUnit>();
            _null = false;
        }

        /// <summary>
        /// 快速查找子对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IUnit this[string name] {
            get {
                var pros = this.GetType().GetProperties();
                foreach (var pro in pros) {
                    if (pro.Name == name && pro.Name != "Item") {
                        var res = (IUnit)pro.GetValue(this);
                        if (eggs.IsNull(res)) return JNull.Create(pro.PropertyType);
                        if (eggs.IsNull(res.GetParent())) {
                            // 填充父对象
                            //var parent = res.GetType().GetProperty("Parent");
                            //parent.SetValue(res, this);
                            res.SetParent(this);
                        }
                        return res;
                    }
                }
                if (list.ContainsKey(name)) return list[name];
                return JNull.Create(typeof(JObject));
            }
            set {
                // 填充父对象
                //var parent = value.GetType().GetProperty("Parent");
                //parent.SetValue(value, this);
                value.SetParent(this);
                var pros = this.GetType().GetProperties();
                foreach (var pro in pros) {
                    if (pro.Name == name && pro.Name != "Item") {
                        if (value.GetUnitType() == UnitTypes.None) {
                            switch (pro.PropertyType.FullName) {
                                case "egg.JsonBean.JBoolean":
                                    JBoolean bValue = JNull.Create(typeof(JBoolean));
                                    pro.SetValue(this, bValue);
                                    break;
                                case "egg.JsonBean.JNumber":
                                    JNumber numValue = JNull.Create(typeof(JNumber));
                                    pro.SetValue(this, numValue);
                                    break;
                                case "egg.JsonBean.JString":
                                    JString szValue = JNull.Create(typeof(JString));
                                    pro.SetValue(this, szValue);
                                    break;
                                default: pro.SetValue(this, null); break;
                            }

                        } else {
                            pro.SetValue(this, value);
                        }

                        return;
                    }
                }
                list[name] = value;
            }
        }

        /// <summary>
        /// 设置元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public JObject Set(string name, string value) {
            this[name] = JString.Create(value);
            return this;
        }

        /// <summary>
        /// 设置元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public JObject Set(string name, double value) {
            this[name] = JNumber.Create(value);
            return this;
        }

        /// <summary>
        /// 设置元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public JObject Set(string name, bool value) {
            this[name] = JBoolean.Create(value);
            return this;
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        public string GetString() { return ToJson(); }

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name) {
            var obj = this[name];
            var tp = obj.GetUnitType();
            switch (tp) {
                case UnitTypes.Boolean: return ((JBoolean)obj).ToString();
                case UnitTypes.Number: return ((JNumber)obj).ToString();
                case UnitTypes.String: return ((JString)obj).ToString();
                case UnitTypes.None: return null;
                default: throw new Exception($"'{tp}'不支持转化为字符串。");
            }
        }

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetNumber(string name) {
            var obj = this[name];
            var tp = obj.GetUnitType();
            switch (tp) {
                case UnitTypes.Boolean: return ((JBoolean)obj).IsTrue() ? 1 : 0;
                case UnitTypes.Number: return ((JNumber)obj).GetNumber();
                case UnitTypes.String: return ((JString)obj).ToString().ToDouble();
                case UnitTypes.None: return 0;
                default: throw new Exception($"'{tp}'不支持转化为数值。");
            }
        }

        /// <summary>
        /// 获取子对象集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetNames() {
            List<string> res = new List<string>();
            var pros = this.GetType().GetProperties();
            foreach (var pro in pros) {
                if (pro.Name != "Item") res.Add(pro.Name);
            }
            foreach (var item in list) {
                res.Add(item.Key);
            }
            return res;
        }

        /// <summary>
        /// 检测子对象名是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            var pros = this.GetType().GetProperties();
            foreach (var pro in pros) {
                if (pro.Name == key) return true;
            }
            return list.ContainsKey(key);
        }

        /// <summary>
        /// 将对象中的内容复制给另一个对象
        /// </summary>
        /// <param name="obj"></param>
        public void CloneTo(JObject obj) {
            var names = GetNames();
            foreach (var name in names) {
                var item = this[name];
                if (eggs.IsNull(item)) {
                    obj[name] = JNull.Create(item.GetType());
                } else {
                    var tp = item.GetUnitType();
                    switch (tp) {
                        case (UnitTypes.Array):
                            if (eggs.IsNull(obj[name])) {
                                obj[name] = this[name];
                            } else {
                                ((JArray)this[name]).CloneTo((JArray)obj[name]);
                            }
                            break;
                        case (UnitTypes.Object):
                            if (eggs.IsNull(obj[name])) {
                                obj[name] = this[name];
                            } else if (obj[name].GetUnitType() == UnitTypes.None) {
                                obj[name] = this[name];
                            } else {
                                ((JObject)this[name]).CloneTo((JObject)obj[name]);
                            }
                            break;
                        default: obj[name] = this[name].Clone(); break;
                    }
                }
            }
        }

        /// <summary>
        /// 从另一个对象中复制对象
        /// </summary>
        /// <param name="obj"></param>
        public void CloneFrom(JObject obj) {
            obj.CloneTo(this);
        }

        /// <summary>
        /// 从另一个对象中复制对象
        /// </summary>
        /// <param name="obj"></param>
        public void CloneFrom(Json.JsonObject obj) {
            foreach (var key in obj.Keys) {
                switch (obj.GetChildUintType(key)) {
                    case Json.UnitType.Object:
                        if (!this.ContainsKey(key)) this[key] = new JObject();
                        ((JObject)this[key]).CloneFrom(obj.Object(key));
                        break;
                    case Json.UnitType.Array:
                        if (!this.ContainsKey(key)) this[key] = new JObject();
                        //if (this.Count <= i) this.Add(new JArray());
                        ((JArray)this[key]).CloneFrom(obj.Array(key));
                        break;
                    case Json.UnitType.Number: this[key] = JNumber.Create(obj.Number(key)); break;
                    case Json.UnitType.String: this[key] = JString.Create(obj.String(key)); break;
                    case Json.UnitType.Boolean: this[key] = JBoolean.Create(obj.Bool(key)); break;
                }
            }
        }

        /// <summary>
        /// 设置Json内容
        /// </summary>
        /// <param name="json"></param>
        public void SetJson(string json) {
            Parser.ParseJson(json, this);
        }

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            eggs.DebugLine(this.GetType().FullName);
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            var pros = this.GetType().GetProperties();
            foreach (var pro in pros) {
                if (pro.PropertyType.IsPublic && pro.Name != "Item") {
                    IUnit item = (IUnit)pro.GetValue(this);
                    // 判断是否为空
                    bool isNull = false;
                    bool visible = true;
                    if (eggs.IsNull(item)) isNull = true;
                    if (!isNull) {
                        if (item.GetUnitType() == UnitTypes.None) isNull = true;
                    }
                    if (isNull) {
                        if (pro.GetCustomAttributes(typeof(JsonOptional), true).Length > 0) {
                            visible = false;
                        }
                    }
                    eggs.DebugLine($"{pro.Name}:{isNull},{visible}");
                    if (visible) {
                        if (sb.Length > 1) sb.Append(',');
                        sb.Append('"');
                        sb.Append(pro.Name);
                        sb.Append('"');
                        sb.Append(':');
                        if (isNull) {
                            sb.Append("NULL");
                        } else {
                            sb.Append(item.ToJson());
                        }
                    }
                }
            }
            foreach (var key in list.Keys) {
                if (sb.Length > 1) sb.Append(',');
                sb.Append('"');
                sb.Append(key);
                sb.Append('"');
                sb.Append(':');
                sb.Append(list[key].ToJson());
            }
            sb.Append('}');
            return sb.ToString();
        }

        /// <summary>
        /// 创建一个同样内容的副本
        /// </summary>
        /// <returns></returns>
        public IUnit Clone() {
            if (_null) return new JObject(_type);
            if (eggs.IsNull(_type)) {
                var tp = this.GetType();
                JObject res = (JObject)tp.Assembly.CreateInstance(tp.FullName);
                this.CloneTo(res);
                return res;
            } else {
                JObject res = (JObject)_type.Assembly.CreateInstance(_type.FullName);
                this.CloneTo(res);
                return res;
            }

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Free() {
            foreach (var item in list) {
                item.Value.Free();
            }
            list.Clear();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Free();
            base.OnDispose();
        }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double GetNumber() {
            throw new NotImplementedException();
        }
    }
}
