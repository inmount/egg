﻿using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 节点
    /// </summary>
    public abstract class Node : egg.BasicObject, egg.Serializable.ISerializable {

        /// <summary>
        /// 获取节点类型
        /// </summary>
        public NodeTypes NodeType { get; private set; }

        /// <summary>
        /// 获取父节点
        /// </summary>
        public Node Parent { get { return (Node)this.BoOwner; } }

        /// <summary>
        /// 实例化一个节点
        /// </summary>
        /// <param name="tp"></param>
        public Node(NodeTypes tp) {
            this.NodeType = tp;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        public void Deserialize(Span<byte> bytes) {
            Deserialize(System.Text.Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Deserialize(string str) {
            OnDeserialize(str);
        }

        /// <summary>
        /// 序列化内容
        /// </summary>
        /// <returns></returns>
        public byte[] SerializeToBytes() {
            return System.Text.Encoding.UTF8.GetBytes(SerializeToString());
        }

        /// <summary>
        /// 序列化内容
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string SerializeToString() {
            return OnSerialize();
        }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected virtual string OnSerialize() { return null; }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected virtual void OnDeserialize(string str) { }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return SerializeToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
        }

        #region [=====对象节点操作接口=====]

        /// <summary>
        /// 判断是否为空节点
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual void OnClear() { throw new Exception($"{this.NodeType.ToString()}类型尚未支持清除子对象"); }

        /// <summary>
        /// 判断是否为空节点
        /// </summary>
        /// <returns></returns>
        public void Clear() { OnClear(); }

        #endregion

        #region [=====对象操作接口=====]

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual Node OnGetKeyItem(string key, bool autoCreate) { throw new Exception($"{this.NodeType.ToString()}类型尚未支持获取索引子对象"); }

        /// <summary>
        /// 设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <exception cref="Exception"></exception>
        protected virtual void OnSetKeyItem(string key, Node node) { throw new Exception($"{this.NodeType.ToString()}类型尚未支持设置索引子对象"); }

        /// <summary>
        /// 获取或设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Node this[string key] { get { return OnGetKeyItem(key, true); } set { OnSetKeyItem(key, value); } }

        /// <summary>
        /// 获取子对象键值集合
        /// </summary>
        /// <returns></returns>
        protected virtual ICollection<string> OnGetKeys() { throw new Exception($"{this.NodeType.ToString()}类型尚未支持获取子对象键值集合"); }

        /// <summary>
        /// 获取子对象键值集合
        /// </summary>
        public ICollection<string> Keys { get { return OnGetKeys(); } }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public Node Null(string key) {
            this[key] = new Null() { };
            return this;
        }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public double Number(string key) { return this[key].ToNumber(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Number(string key, double value) { this[key] = new Number() { Value = value }; return this; }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Boolean(string key, bool value) { this[key] = new Boolean() { Value = value }; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public bool Boolean(string key) { return this[key].ToBoolean(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node String(string key, string value) { this[key] = new String() { Value = value }; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public string String(string key) { return this[key].ToString(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Object(string key, Object value) { this[key] = value; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public Object Object(string key) {
            Node node = OnGetKeyItem(key, false);
            if (node.IsNull() || node.IsNullNode()) {
                OnSetKeyItem(key, new Object());
                return (Object)OnGetKeyItem(key, false);
            }
            return (Object)node;
        }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node List(string key, List value) { this[key] = value; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public List List(string key) {
            Node node = OnGetKeyItem(key, false);
            if (node.IsNull() || node.IsNullNode()) {
                OnSetKeyItem(key, new List());
                return (List)OnGetKeyItem(key, false);
            }
            return (List)node;
        }

        /// <summary>
        /// 设置Json内容
        /// </summary>
        /// <returns></returns>
        public Node Json(string key, string json) { this[key] = eggs.Json.Parse(json); return this; }

        #endregion

        #region [=====数组操作接口=====]

        /// <summary>
        /// 获取索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual Node OnGetIndexItem(int index, bool autoCreate) { throw new Exception($"{this.NodeType.ToString()}类型尚未支持获取索引对象"); }

        /// <summary>
        /// 设置索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        /// <exception cref="Exception"></exception>
        protected virtual void OnSetIndexItem(int index, Node node) { throw new Exception($"{this.NodeType.ToString()}类型尚未支持设置索引对象"); }

        /// <summary>
        /// 获取或设置数组对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Node this[int index] { get { return OnGetIndexItem(index, true); } set { this.OnSetIndexItem(index, value); } }

        /// <summary>
        /// 获取索引对象数量集合
        /// </summary>
        /// <returns></returns>
        protected virtual int OnGetItemCount() { throw new Exception($"{this.NodeType.ToString()}类型尚未支持索引对象数量"); }

        /// <summary>
        /// 获取索引对象数量集合
        /// </summary>
        public int Count { get { return OnGetItemCount(); } }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public Node Null(int index) {
            this[index] = new Null() { };
            return this;
        }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public double Number(int index) { return this[index].ToNumber(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Number(int index, double value) { this[index] = new Number() { Value = value }; return this; }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Boolean(int index, bool value) { this[index] = new Boolean() { Value = value }; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public bool Boolean(int index) { return this[index].ToBoolean(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node String(int index, string value) { this[index] = new String() { Value = value }; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public string String(int index) { return this[index].ToString(); }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node Object(int index, Object value) { this[index] = value; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public Object Object(int index) {
            Node node = OnGetIndexItem(index, false);
            if (node.IsNull() || node.IsNullNode()) {
                OnSetIndexItem(index, new Object());
                return (Object)OnGetIndexItem(index, false);
            }
            return (Object)node;
        }

        /// <summary>
        /// 设置数值内容
        /// </summary>
        /// <returns></returns>
        public Node List(int index, List value) { this[index] = value; return this; }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        public List List(int index) {
            Node node = OnGetIndexItem(index, false);
            if (node.IsNull() || node.IsNullNode()) {
                OnSetIndexItem(index, new List());
                return (List)OnGetIndexItem(index, false);
            }
            return (List)node;
        }

        /// <summary>
        /// 设置Json内容
        /// </summary>
        /// <returns></returns>
        public Node Json(int index, string json) { this[index] = eggs.Json.Parse(json); return this; }

        #endregion

        #region [=====值节点操作接口=====]

        /// <summary>
        /// 判断是否为空节点
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual bool OnCheckNull() { return false; }

        /// <summary>
        /// 判断是否为空节点
        /// </summary>
        /// <returns></returns>
        public bool IsNullNode() { return OnCheckNull(); }

        /// <summary>
        /// 转化为数值
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual double OnParseNumber() { throw new Exception($"{this.NodeType.ToString()}类型尚未支持转化为数值内容"); }

        /// <summary>
        /// 转化为数值
        /// </summary>
        /// <returns></returns>
        public double ToNumber() { return OnParseNumber(); }

        /// <summary>
        /// 转化为数值
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual bool OnParseBoolean() { throw new Exception($"{this.NodeType.ToString()}类型尚未支持转化为数值内容"); }

        /// <summary>
        /// 转化为数值
        /// </summary>
        /// <returns></returns>
        public bool ToBoolean() { return OnParseBoolean(); }


        #endregion

        #region [=====赋值转化接口=====]

        /// <summary>
        /// 赋值为布尔型
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator bool(Node node) {
            return node.ToBoolean();
        }

        /// <summary>
        /// 从布尔型建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(bool value) {
            return new Boolean() { Value = value };
        }

        /// <summary>
        /// 赋值为字节型数值
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator byte(Node node) {
            return (byte)node.ToNumber();
        }

        /// <summary>
        /// 从字节型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(byte value) {
            return new Number() { Value = value };
        }

        /// <summary>
        /// 赋值为整型数值
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator int(Node node) {
            return (int)node.ToNumber();
        }

        /// <summary>
        /// 从整型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(int value) {
            return new Number() { Value = value };
        }

        /// <summary>
        /// 赋值为长整型数值
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator long(Node node) {
            return (long)node.ToNumber();
        }

        /// <summary>
        /// 从长整型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(long value) {
            return new Number() { Value = value };
        }

        /// <summary>
        /// 赋值为浮点型数值
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator float(Node node) {
            return (float)node.ToNumber();
        }

        /// <summary>
        /// 从浮点型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(float value) {
            return new Number() { Value = value };
        }

        /// <summary>
        /// 赋值为双精度浮点型数值
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator double(Node node) {
            return node.ToNumber();
        }

        /// <summary>
        /// 从双精度浮点型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(double value) {
            return new Number() { Value = value };
        }

        /// <summary>
        /// 赋值为字符串
        /// </summary>
        /// <param name="node">节点</param>
        public static implicit operator string(Node node) {
            return node.ToString();
        }

        /// <summary>
        /// 从字符串建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Node(string value) {
            return new String() { Value = value };
        }

        #endregion

    }
}
