using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace egg.Net {

    /// <summary>
    /// TCP协议服务器通讯实体集合
    /// </summary>
    public class TcpServerEntities : List<TcpServerEntity> {

        /// <summary>
        /// 清理
        /// </summary>
        public void Clean() {
            for (int i = this.Count - 1; i >= 0; i--) {
                if (!this[i].Connected) this.RemoveAt(i);
            }
        }

    }
}
