using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 单元类型
    /// </summary>
    public enum UnitType {

        /// <summary>
        /// 空
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 数值类型
        /// </summary>
        Number = 0x01,

        /// <summary>
        /// 字符串类型
        /// </summary>
        String = 0x02,

        /// <summary>
        /// 字符串类型
        /// </summary>
        Boolean = 0x03,

        /// <summary>
        /// 对象类型
        /// </summary>
        Object = 0x11,

        /// <summary>
        /// 数组类型
        /// </summary>
        Array = 0x21
    }

    /// <summary>
    /// Json存储单元
    /// </summary>
    public abstract class JsonUnit : egg.Object {

        /// <summary>
        /// 获取单元类型
        /// </summary>
        public UnitType UnitType { get; private set; }

        /// <summary>
        /// 获取父对象
        /// </summary>
        public JsonUnit Parent { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        public JsonUnit(UnitType type, JsonUnit parent = null) {
            this.UnitType = type;
            this.Parent = parent;
        }

        #region [=====操作接口=====]

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected virtual double OnGetNumber() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取数值内容"); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double GetNumber() { return OnGetNumber(); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        protected virtual void OnSetNumber(double value) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持设置数值内容"); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        public void SetValue(double value) { this.OnSetNumber(value); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnGetBoolean() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取数值内容"); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public bool GetBoolean() { return OnGetBoolean(); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        protected virtual void OnSetBoolean(bool value) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持设置数值内容"); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        public void SetBoolean(bool value) { this.OnSetBoolean(value); }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetString() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取字符串内容"); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public string GetString() { return OnGetString(); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        protected virtual void OnSetString(string value) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持设置字符串内容"); }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <returns></returns>
        public void SetValue(string value) { this.OnSetString(value); }

        /// <summary>
        /// 获取Json标准字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetJsonString() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取Json标准字符串"); }

        /// <summary>
        /// 获取Json标准字符串
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() { return OnGetJsonString(); }

        /// <summary>
        /// 获取索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual JsonUnit OnGetArrayItem(int index) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取索引对象"); }

        /// <summary>
        /// 设置索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected virtual void OnSetArrayItem(int index, JsonUnit unit) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持设置索引对象"); }

        /// <summary>
        /// 获取或设置数组对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonObject this[int index] { get { return this.Object(index); } set { this.OnSetArrayItem(index, value); } }

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual JsonUnit OnGetChildItem(string key) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取索引子对象"); }

        /// <summary>
        /// 设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected virtual void OnSetChildItem(string key, JsonUnit unit) { throw new Exception($"{this.UnitType.ToString()}类型尚未支持设置索引子对象"); }

        /// <summary>
        /// 获取或设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonObject this[string key] { get { return this.Object(key); } set { this.OnSetChildItem(key, value); } }

        /// <summary>
        /// 获取子对象键值集合
        /// </summary>
        /// <returns></returns>
        protected virtual ICollection<string> OnGetKeys() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持获取子对象键值集合"); }

        /// <summary>
        /// 获取子对象键值集合
        /// </summary>
        public ICollection<string> Keys { get { return OnGetKeys(); } }

        /// <summary>
        /// 获取索引对象数量集合
        /// </summary>
        /// <returns></returns>
        protected virtual int OnGetItemCount() { throw new Exception($"{this.UnitType.ToString()}类型尚未支持索引对象数量"); }

        /// <summary>
        /// 获取索引对象数量集合
        /// </summary>
        public int Count { get { return OnGetItemCount(); } }

        #endregion

        #region [=====快捷操作入口=====]

        /// <summary>
        /// 获取子元素类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public UnitType GetChildUintType(string key) {
            var res = OnGetChildItem(key);
            if (eggs.IsNull(res)) return UnitType.None;
            return res.UnitType;
        }

        /// <summary>
        /// 获取子元素类型
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UnitType GetChildUintType(int index) {
            var res = OnGetArrayItem(index);
            if (eggs.IsNull(res)) return UnitType.None;
            return res.UnitType;
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonObject Object(string key) {
            var res = OnGetChildItem(key);
            if (res == null) {
                var obj = new JsonObject(this);
                OnSetChildItem(key, obj);
                return obj;
            } else {
                return (JsonObject)res;
            }
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonObject Object(int index) {
            var res = OnGetArrayItem(index);
            if (res == null) {
                var obj = new JsonObject(this);
                OnSetArrayItem(index, obj);
                return obj;
            } else {
                return (JsonObject)res;
            }
        }

        /// <summary>
        /// 获取一个数组
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonArray Array(string key) {
            var res = OnGetChildItem(key);
            if (res == null) {
                var obj = new JsonArray(this);
                OnSetChildItem(key, obj);
                return obj;
            } else {
                return (JsonArray)res;
            }
        }

        /// <summary>
        /// 获取一个数组
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonArray Array(int index) {
            var res = OnGetArrayItem(index);
            if (res == null) {
                var obj = new JsonArray(this);
                OnSetArrayItem(index, obj);
                return obj;
            } else {
                return (JsonArray)res;
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string String(string key) {
            var res = OnGetChildItem(key);
            if (res == null) {
                var obj = new JsonString(this);
                OnSetChildItem(key, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value;
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return $"{objNumber.Value}";
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return $"{objBool.Value}";
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string String(int index) {
            var res = OnGetArrayItem(index);
            if (res == null) {
                var obj = new JsonString(this);
                OnSetArrayItem(index, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value;
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return $"{objNumber.Value}";
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return $"{objBool.Value}";
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 设置一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit String(string key, string val) {
            var obj = new JsonString(this);
            obj.Value = val;
            OnSetChildItem(key, obj);
            return this;
        }

        /// <summary>
        /// 设置一个字符串
        /// </summary>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit String(int index, string val) {
            var obj = new JsonString(this);
            obj.Value = val;
            OnSetArrayItem(index, obj);
            return this;
        }

        /// <summary>
        /// 获取一个布尔型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Bool(string key) {
            var res = OnGetChildItem(key);
            if (res == null) {
                var obj = new JsonBoolean(this);
                OnSetChildItem(key, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value.ToLower() == "ture";
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return objNumber.Value > 0;
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return objBool.Value;
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Bool(int index) {
            var res = OnGetArrayItem(index);
            if (res == null) {
                var obj = new JsonBoolean(this);
                OnSetArrayItem(index, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value.ToLower() == "ture";
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return objNumber.Value > 0;
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return objBool.Value;
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 设置一个布尔型数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit Bool(string key, bool val) {
            var obj = new JsonBoolean(this);
            obj.Value = val;
            OnSetChildItem(key, obj);
            return this;
        }

        /// <summary>
        /// 设置一个字符串
        /// </summary>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit Bool(int index, bool val) {
            var obj = new JsonBoolean(this);
            obj.Value = val;
            OnSetArrayItem(index, obj);
            return this;
        }

        /// <summary>
        /// 获取一个数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double Number(string key) {
            var res = OnGetChildItem(key);
            if (res == null) {
                var obj = new JsonNumber(this);
                OnSetChildItem(key, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value.ToDouble();
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return objNumber.Value;
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return objBool.Value ? 1 : 0;
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 获取一个数值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double Number(int index) {
            var res = OnGetArrayItem(index);
            if (res == null) {
                var obj = new JsonNumber(this);
                OnSetArrayItem(index, obj);
                return obj.Value;
            } else {
                string objType = res.GetType().FullName;
                switch (objType) {
                    case "egg.Json.JsonString":
                        var obj = (JsonString)res;
                        return obj.Value.ToDouble();
                    case "egg.Json.JsonNumber":
                        var objNumber = (JsonNumber)res;
                        return objNumber.Value;
                    case "egg.Json.JsonBoolean":
                        var objBool = (JsonBoolean)res;
                        return objBool.Value ? 1 : 0;
                    default:
                        throw new Exception("操作类型错误");
                }
            }
        }

        /// <summary>
        /// 设置一个数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit Number(string key, double val) {
            var obj = new JsonNumber(this);
            obj.Value = val;
            OnSetChildItem(key, obj);
            return this;
        }

        /// <summary>
        /// 设置一个数值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public JsonUnit Number(int index, double val) {
            var obj = new JsonNumber(this);
            obj.Value = val;
            OnSetArrayItem(index, obj);
            return this;
        }

        private JsonNumberOperator _number = null;

        /// <summary>
        /// 获取数值操作器
        /// </summary>
        public JsonNumberOperator Num {
            get {
                if (_number == null) _number = new JsonNumberOperator(this);
                return _number;
            }
        }

        /// <summary>
        /// 获取数值操作器
        /// </summary>
        public JsonNumberOperator Dbl {
            get {
                if (_number == null) _number = new JsonNumberOperator(this);
                return _number;
            }
        }

        private JsonIntOperator _int = null;

        /// <summary>
        /// 获取数值操作器
        /// </summary>
        public JsonIntOperator Int {
            get {
                if (_int == null) _int = new JsonIntOperator(this);
                return _int;
            }
        }

        private JsonLongOperator _long = null;

        /// <summary>
        /// 获取数值操作器
        /// </summary>
        public JsonLongOperator Lng {
            get {
                if (_long == null) _long = new JsonLongOperator(this);
                return _long;
            }
        }

        private JsonFloatOperator _float = null;

        /// <summary>
        /// 获取数值操作器
        /// </summary>
        public JsonFloatOperator Flo {
            get {
                if (_float == null) _float = new JsonFloatOperator(this);
                return _float;
            }
        }

        private JsonStringOperator _string = null;

        /// <summary>
        /// 获取字符串操作器
        /// </summary>
        public JsonStringOperator Str {
            get {
                if (_string == null) _string = new JsonStringOperator(this);
                return _string;
            }
        }

        private JsonBoolOperator _bool = null;

        /// <summary>
        /// 获取字符串操作器
        /// </summary>
        public JsonBoolOperator Bol {
            get {
                if (_bool == null) _bool = new JsonBoolOperator(this);
                return _bool;
            }
        }

        private JsonObjectOperator _object = null;

        /// <summary>
        /// 获取对象操作器
        /// </summary>
        public JsonObjectOperator Obj {
            get {
                if (_object == null) _object = new JsonObjectOperator(this);
                return _object;
            }
        }

        private JsonArrayOperator _array = null;

        /// <summary>
        /// 获取对象操作器
        /// </summary>
        public JsonArrayOperator Arr {
            get {
                if (_array == null) _array = new JsonArrayOperator(this);
                return _array;
            }
        }

        #endregion

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return OnGetJsonString();
        }

    }
}
