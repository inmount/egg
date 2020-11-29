using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace egg.Net {

    /// <summary>
    /// TCP协议服务器客户端
    /// </summary>
    public class TcpClient : TcpConnection {

        #region [=====连接事件处理=====]

        // 事件绑定
        private List<Action> connectActions;

        /// <summary>
        /// 注册一个开始事件
        /// </summary>
        /// <param name="action"></param>
        public void OnConnect(Action action) {
            if (action == null) return;
            connectActions.Add(action);
        }

        /// <summary>
        /// 触发开始事件
        /// </summary>
        protected void OnConnect() {
            for (int i = 0; i < connectActions.Count; i++) {
                if (connectActions[i] != null) connectActions[i]();
            }
        }

        #endregion

        // 连接信息
        private string _host;
        private int _port;

        /// <summary>
        /// 实例化一个新的TCP客户端
        /// </summary>
        /// <param name="host">主服务器地址</param>
        /// <param name="port">主服务器服务端口</param>
        public TcpClient(string host, int port) {
            _host = host;
            _port = port;
            connectActions = new List<Action>();
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect() {
            try {
                // 初始化基础网络通讯组件并连接
                base.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                base.Socket.Connect(_host, _port);
                // 触发开始事件
                this.OnConnect();
                // 设置连接状态为已连接
                base.Connected = true;
            } catch (Exception ex) {
                base.OnError(ex.Message, 0, ex.ToString());
                this.OnDisconnect();
            }
        }

    }
}
