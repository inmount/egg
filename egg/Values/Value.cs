using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Values {

    /// <summary>
    /// 值基类
    /// </summary>
    public abstract class Value : egg.Basic {

        #region [=====空值处理=====]

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsEmpty() { return OnCheckEmpty(); }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnCheckEmpty() { return false; }

        /// <summary>
        /// 获取一个空对象
        /// </summary>
        public static None None { get { return new None(); } }

        #endregion

        #region [=====字符串处理=====]

        /// <summary>
        /// 赋值为字符串数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator string(Value value) {
            return value.ToString();
        }

        /// <summary>
        /// 从字符串数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(string value) {
            return new String(value);
        }

        #endregion

        #region [=====对象处理=====]

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T ToObject<T>() { return (T)OnParseObject(); }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        protected virtual object OnParseObject() { throw new NotImplementedException(); }

        #endregion

        #region [=====布尔数值处理=====]

        /// <summary>
        /// 获取布尔型表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ToBealoon() { return OnParseBealoon(); }

        /// <summary>
        /// 获取布尔型表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnParseBealoon() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为布尔型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator bool(Value value) {
            return value.ToBealoon();
        }

        /// <summary>
        /// 从布尔型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(bool value) {
            return new Bealoon(value);
        }

        #endregion

        #region [=====字节数值处理=====]

        /// <summary>
        /// 获取字节型表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public byte ToByte() { return OnParseByte(); }

        /// <summary>
        /// 获取字节型表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual byte OnParseByte() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为字节型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator byte(Value value) {
            return value.ToByte();
        }

        /// <summary>
        /// 从字节型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(byte value) {
            return new Byte(value);
        }

        #endregion

        #region [=====整型数值处理=====]

        /// <summary>
        /// 获取整型表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int ToInteger() { return OnParseInteger(); }

        /// <summary>
        /// 获取整型表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual int OnParseInteger() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为整型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator int(Value value) {
            return value.ToInteger();
        }

        /// <summary>
        /// 从整型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(int value) {
            return new Integer(value);
        }

        #endregion

        #region [=====长整型数值处理=====]

        /// <summary>
        /// 获取长整型表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long ToLong() { return OnParseLong(); }

        /// <summary>
        /// 获取长整型表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual long OnParseLong() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为长整型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator long(Value value) {
            return value.ToLong();
        }

        /// <summary>
        /// 从长整型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(long value) {
            return new Long(value);
        }

        #endregion

        #region [=====单精度数值处理=====]

        /// <summary>
        /// 获取单精度表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public float ToFloat() { return OnParseFloat(); }

        /// <summary>
        /// 获取单精度表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual float OnParseFloat() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为单精度浮点型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator float(Value value) {
            return value.ToFloat();
        }

        /// <summary>
        /// 从单精度浮点型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(float value) {
            return new Float(value);
        }

        #endregion

        #region [=====双精度数值处理=====]

        /// <summary>
        /// 获取双精度表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsNumber() { return OnCheckDouble(); }

        /// <summary>
        /// 获取双精度表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnCheckDouble() { return false; }

        /// <summary>
        /// 获取双精度表示形式
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double ToDouble() { return OnParseDouble(); }

        /// <summary>
        /// 获取双精度表示形式
        /// </summary>
        /// <returns></returns>
        protected virtual double OnParseDouble() { throw new NotImplementedException(); }

        /// <summary>
        /// 赋值为双精度浮点型数值
        /// </summary>
        /// <param name="value">值类型</param>
        public static implicit operator double(Value value) {
            return value.ToDouble();
        }

        /// <summary>
        /// 从双精度浮点型数据建立对象
        /// </summary>
        /// <param name="value">内容</param>
        public static implicit operator Value(double value) {
            return new Double(value);
        }

        #endregion

    }
}
