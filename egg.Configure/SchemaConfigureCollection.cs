using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace egg.Configure {

    /// <summary>
    /// 带分类的配置集合
    /// </summary>
    public class SchemaConfigureCollection : KeyList<KeyList<object>>, IConfigureCollection {

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetConfigure(string key) {
            string[] keys = key.Split('/');
            if (keys.Length != 2) throw new Exception($"键名称'{key}'不可用");
            if (!base.ContainsKey(keys[0])) return null;
            return base[keys[0]][keys[1]];
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetSection(string key, Type type) {
            // 判断节点是否存在，存在则闯将
            if (!base.ContainsKey(key)) return Activator.CreateInstance(type);
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(base[key]));
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T GetSection<T>(string key) where T : new() {
            // 判断节点是否存在，存在则闯将
            if (!base.ContainsKey(key)) return new T();
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(base[key]));
        }


        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetConfigure(string key, object value) {
            string[] keys = key.Split('/');
            if (keys.Length != 2) throw new Exception($"键名称'{key}'不可用");
            if (!base.ContainsKey(keys[0])) base[keys[0]] = new KeyList<object>();
            base[keys[0]][keys[1]] = value;
        }

        /// <summary>
        /// 设置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetSection(string key, object obj) {
            // 判断节点是否存在，存在则闯将
            if (!base.ContainsKey(key)) base[key] = new KeyList<object>();
            // 获取节点
            var section = base[key];
            var tp = obj.GetType();
            foreach (var pro in tp.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)) {
                string name = pro.Name;
                var dms = pro.GetCustomAttributes<DataMemberAttribute>();
                foreach (var dm in dms) {
                    if (!string.IsNullOrEmpty(dm.Name)) {
                        name = dm.Name;
                    }
                }
                // 获取值
                section[name] = pro.GetValue(obj);
            }
        }

        /// <summary>
        /// 添加Json文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SchemaConfigureCollection AddJsonFile(string path) {
            string content = eggs.IO.GetUtf8FileContent(path);
            var obj = JObject.Parse(content);
            foreach (var item in obj) {
                if (!base.ContainsKey(item.Key)) base[item.Key] = new KeyList<object>();
                // 获取节点
                var section = base[item.Key];
                // 获取Json对象
                var objs = (JObject)obj[item.Key];
                foreach (var items in objs) {
                    section[items.Key] = objs[items.Key];
                }
            }
            return this;
        }
    }
}
