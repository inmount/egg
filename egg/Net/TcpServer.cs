using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace egg.Net {

    /// <summary>
    /// TCP协议服务器
    /// </summary>
    public class TcpServer : egg.BasicObject {

        #region [=====开始事件处理=====]

        // 事件绑定
        private List<Action> startActions;

        /// <summary>
        /// 注册一个开始事件
        /// </summary>
        /// <param name="action"></param>
        public void OnStart(Action action) {
            if (action == null) return;
            startActions.Add(action);
        }

        /// <summary>
        /// 触发开始事件
        /// </summary>
        protected void OnStart() {
            for (int i = 0; i < startActions.Count; i++) {
                if (startActions[i] != null) startActions[i]();
            }
        }

        #endregion

        #region [=====错误事件处理=====]

        // 事件绑定
        private List<Action<TcpErrorActionArgs>> errActions;

        /// <summary>
        /// 注册一个错误事件
        /// </summary>
        /// <param name="action"></param>
        public void OnError(Action<TcpErrorActionArgs> action) {
            if (action == null) return;
            errActions.Add(action);
        }

        /// <summary>
        /// 触发错误事件
        /// </summary>
        protected void OnError(string msg, int code = 0, string detail = null) {
            var args = new TcpErrorActionArgs() {
                Code = code,
                Message = msg,
                Detail = detail
            };
            for (int i = 0; i < errActions.Count; i++) {
                if (errActions[i] != null) errActions[i](args);
            }
        }

        #endregion

        #region [=====停止事件处理=====]

        // 事件绑定
        private List<Action> stopActions;

        /// <summary>
        /// 注册一个停止事件
        /// </summary>
        /// <param name="action"></param>
        public void OnStop(Action action) {
            if (action == null) return;
            stopActions.Add(action);
        }

        /// <summary>
        /// 触发停止事件
        /// </summary>
        protected void OnStop() {
            for (int i = 0; i < stopActions.Count; i++) {
                if (stopActions[i] != null) stopActions[i]();
            }
        }

        #endregion

        #region [=====新连接事件处理=====]

        // 事件绑定
        private List<Action<TcpAcceptActionArgs>> acceptActions;

        /// <summary>
        /// 注册一个新连接事件
        /// </summary>
        /// <param name="action"></param>
        public void OnAccept(Action<TcpAcceptActionArgs> action) {
            if (action == null) return;
            acceptActions.Add(action);
        }

        /// <summary>
        /// 触发新连接事件
        /// </summary>
        protected void OnAccept(Socket socket) {
            TcpAcceptActionArgs args = new TcpAcceptActionArgs() { Socket = socket };
            for (int i = 0; i < acceptActions.Count; i++) {
                if (acceptActions[i] != null) acceptActions[i](args);
            }
        }

        #endregion

        // 基础网络通讯组件
        private Socket _socket;
        private IPAddress _ip;
        private int _port;
        private long _indexer;

        /// <summary>
        /// 获取工作标识
        /// </summary>        
        public bool Working { get; private set; }

        /// <summary>
        /// 获取通讯实体集合
        /// </summary>
        public TcpServerEntities Entities { get; private set; }

        /// <summary>
        /// 实例化一个新的TCP服务端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public TcpServer(IPAddress ip, int port) {
            _ip = ip;
            _port = port;
            this.Working = false;
            this.Entities = new TcpServerEntities();
            acceptActions = new List<Action<TcpAcceptActionArgs>>();
            startActions = new List<Action>();
            errActions = new List<Action<TcpErrorActionArgs>>();
            stopActions = new List<Action>();
        }

        /// <summary>
        /// 获取一个新的索引
        /// </summary>
        /// <returns></returns>
        public long GetNewIndex() { return ++_indexer; }

        // 接受连接
        private void Socket_Accept(IAsyncResult result) {
            // 判断并设置工作标识
            if (!this.Working) return;
            // 清理过期连接
            this.Entities.Clean();
            //获取响应的通讯组件
            Socket client = _socket.EndAccept(result);
            try {
                //处理下一个客户端连接
                _socket.BeginAccept(new AsyncCallback(Socket_Accept), _socket);
                // 新增一个实体
                this.OnAccept(client);
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start() {
            if (this.Working) return;
            try {
                // 实例化基础网络通讯组件
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(_ip, _port));
                // 监听基础网络通讯组件
                _socket.Listen(10);
                this.Working = true;
                this.OnStart();
                // 开始接受连接
                _socket.BeginAccept(Socket_Accept, _socket);
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
                this.OnStop();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop() {
            if (!this.Working) return;
            try {
                if (_socket != null) {
                    _socket.Close();
                    _socket = null;
                    this.Working = false;
                    this.OnStop();
                }
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
            }
            // 清空所有连接实体
            for (int i = 0; i < this.Entities.Count; i++) {
                var entity = this.Entities[i];
                if (entity.Connected) entity.Close();
            }
            this.Entities.Clear();
        }

    }
}
