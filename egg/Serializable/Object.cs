using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 可序列化对象
    /// </summary>
    public class Object : ISerializable, IDisposable {

        /// <summary>
        /// 获取对象内元素集合
        /// </summary>
        private List<ISerializable> _items;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <returns></returns>
        public Object() {
            _items = new List<ISerializable>();
        }

        /// <summary>
        /// 注册一个可序列化对象
        /// </summary>
        /// <param name="item"></param>
        public void Reg(ISerializable item) {
            _items.Add(item);
        }

        /// <summary>
        /// 获取存储尺寸
        /// </summary>
        public int Size {
            get {
                int res = 4;
                foreach (var item in _items) {
                    res += item.Size;
                }
                return res;
            }
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            _items.Clear();
            _items = null;
        }

        /// <summary>
        /// 内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetValue(byte[] val, int offset = 0, int count = 0) {

            // 读取数据长度
            int size = BitConverter.ToInt32(val, offset);
            offset += 4;

            // 按顺序设置内容
            foreach (var item in _items) {

                // 建立数组片段
                item.SetValue(val, offset, val.Length - offset);
                offset += item.Size;

            }

        }

        /// <summary>
        /// 获取字节数字表示形式
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            List<byte> ls = new List<byte>();
            // 添加长度信息
            ls.AddRange(BitConverter.GetBytes(this.Size - 4));
            // 按顺序填充字节数组
            foreach (var item in _items) {
                ls.AddRange(item.ToBytes());
            }
            return ls.ToArray();
        }
    }
}
