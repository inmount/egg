﻿using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数值对象操作器
    /// </summary>
    public class JsonFloatOperator {

        private JsonUnit _json;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="jsonUnit"></param>
        public JsonFloatOperator(JsonUnit jsonUnit) {
            _json = jsonUnit;
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float this[int index] {
            get {
                if (_json.Count <= index + 1) {
                    return (float)_json.Number(index);
                }
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
        public float this[string key] {
            get {
                if (_json.Keys.Contains(key)) {
                    return (float)_json.Number(key);
                }
                return 0;
            }
            set {
                _json.Number(key, value);
            }
        }

    }
}
