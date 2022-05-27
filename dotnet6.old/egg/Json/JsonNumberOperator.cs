using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数值对象操作器
    /// </summary>
    public class JsonNumberOperator {

        private JsonUnit _json;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="jsonUnit"></param>
        public JsonNumberOperator(JsonUnit jsonUnit) {
            _json = jsonUnit;
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index] {
            get {
                var obj = _json[index];
                if (obj != null) return obj.GetNumber();
                return 0;
            }
            set {
                _json.Number(index, value);
            }
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double this[string key] {
            get {
                if (_json.Keys.Contains(key)) {
                    return _json.Number(key);
                }
                return 0;
            }
            set {
                _json.Number(key, value);
            }
        }

    }
}
