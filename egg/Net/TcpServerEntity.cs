using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace egg.Net {

    /// <summary>
    /// TCP协议服务器通讯实体
    /// </summary>
    public class TcpServerEntity : TcpConnection {

        /// <summary>
        /// 获取标识符
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 获取关联服务端
        /// </summary>
        public TcpServer Server { get; private set; }

        /// <summary>
        /// 实例化一个新的TCP服务端实体
        /// </summary>
        /// <param name="server"></param>
        /// <param name="socket"></param>
        public TcpServerEntity(TcpServer server, Socket socket) {
            this.Server = server;
            this.Id = server.GetNewIndex();
            base.Socket = socket;
            this.Connected = true;
        }

    }
}
