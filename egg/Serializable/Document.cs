using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {
    /// <summary>
    /// 序列化文档
    /// </summary>
    public abstract class Document : egg.BasicObject, ISerializable {

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        protected virtual void OnDeserialize(Span<byte> bytes) { }

        /// <summary>
        /// 序列化
        /// </summary>
        protected virtual byte[] OnSerializeToBytes() { return null; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        public void Deserialize(Span<byte> bytes) {
            this.OnDeserialize(bytes);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public byte[] SerializeToBytes() {
            return OnSerializeToBytes();
        }

        void ISerializable.Deserialize(string str) {
            throw new NotImplementedException();
        }

        string ISerializable.SerializeToString() {
            throw new NotImplementedException();
        }
    }
}
