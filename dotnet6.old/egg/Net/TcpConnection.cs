using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace egg.Net {

    /// <summary>
    /// TCP连接
    /// </summary>
    public class TcpConnection : egg.Object {

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

        #region [=====空闲事件处理=====]

        // 事件绑定
        private List<Action> freeActions;

        /// <summary>
        /// 注册一个错误事件
        /// </summary>
        /// <param name="action"></param>
        public void OnFree(Action action) {
            if (action == null) return;
            freeActions.Add(action);
        }

        /// <summary>
        /// 触发错误事件
        /// </summary>
        protected void OnFree() {
            for (int i = 0; i < freeActions.Count; i++) {
                if (freeActions[i] != null) freeActions[i]();
            }
        }

        #endregion

        #region [=====断开连接事件处理=====]

        // 事件绑定
        private List<Action> disconnectActions;

        /// <summary>
        /// 注册一个开始事件
        /// </summary>
        /// <param name="action"></param>
        public void OnDisconnect(Action action) {
            if (action == null) return;
            disconnectActions.Add(action);
        }

        /// <summary>
        /// 触发开始事件
        /// </summary>
        protected void OnDisconnect() {
            for (int i = 0; i < disconnectActions.Count; i++) {
                if (disconnectActions[i] != null) disconnectActions[i]();
            }
        }

        #endregion

        // 基础网络通讯组件
        private Socket _socket;
        // 计时器
        private int _tick;
        // 空闲检测线程
        private Thread _free_thread;
        // 工作状态
        private bool _free_working;

        /// <summary>
        /// 获取基础网络通讯组件
        /// </summary>
        public Socket Socket {
            get { return _socket; }
            protected set {
                _socket = value;
            }
        }

        /// <summary>
        /// 获取空闲秒数
        /// </summary>
        public int FreeSecond { get; protected set; }

        /// <summary>
        /// 获取是否连接
        /// </summary>
        public bool Connected { get; protected set; }

        /// <summary>
        /// 实例化一个新的TCP连接
        /// </summary>
        public TcpConnection() {
            _socket = null;
            this.Connected = false;
            freeActions = new List<Action>();
            errActions = new List<Action<TcpErrorActionArgs>>();
            disconnectActions = new List<Action>();
            // 设置空闲秒数为不触发
            this.FreeSecond = 0;
            // 进行空闲时间监视
            UpdateFreeTime();
            _free_working = true;
            _free_thread = new Thread(FreeThread);
            _free_thread.Start();
        }

        // 空闲监控线程
        private void FreeThread() {
            while (_free_working) {
                Thread.Sleep(10);
                // 判断是否需要执行空闲事件
                if (this.FreeSecond > 0 && _socket != null) {
                    int sec = GetFreeTime() / 1000;
                    if (sec >= this.FreeSecond) {
                        // 更新空闲时间
                        UpdateFreeTime();
                        // 触发空闲事件
                        this.OnFree();
                    }
                }
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close() {
            if (!this.Connected) return;
            try {
                // 关闭连接
                if (_socket != null) {
                    _socket.Close();
                }
            } catch (Exception ex) { this.OnError(ex.Message, 0, ex.ToString()); }
            _socket = null;
            this.Connected = false;
            this.OnDisconnect();
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="len"></param>
        /// <param name="action"></param>
        public void Receive(int len, Action<ArraySegment<byte>> action) {
            byte[] buffer = new byte[len];
            int res = Receive(buffer, 0, len);
            if (res <= 0) return;
            if (action != null) action(new ArraySegment<byte>(buffer, 0, res));
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int Receive(byte[] buffer, int offset, int len) {
            try {
                // 接收数据
                int res = _socket.Receive(buffer, offset, len, SocketFlags.None);
                // 更新空闲计时
                UpdateFreeTime();
                return res;
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
                this.Close();
                return 0;
            }
        }

        /// <summary>
        /// 直接向服务端发送数据
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes) {
            try {
                // 发送数据
                _socket.Send(bytes);
                // 更新空闲计时
                UpdateFreeTime();
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
                this.Close();
            }
        }

        /// <summary>
        /// 直接向服务端发送数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        public void Send(byte[] bytes, int pos, int length) {
            try {
                // 发送数据
                _socket.Send(bytes, pos, length, SocketFlags.None);
                // 更新空闲计时
                UpdateFreeTime();
            } catch (Exception ex) {
                this.OnError(ex.Message, 0, ex.ToString());
                this.Close();
            }
        }

        /// <summary>
        /// 获取空闲时间(毫秒)
        /// </summary>
        /// <returns></returns>
        public int GetFreeTime() { return Environment.TickCount - _tick; }

        /// <summary>
        /// 更新空闲
        /// </summary>
        /// <returns></returns>
        public void UpdateFreeTime() { _tick = Environment.TickCount; }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            this.Close();
            _free_working = false;
            try {
                // 空闲线程清理
                if (_free_thread != null) _free_thread.Abort();
            } catch (Exception ex) { this.OnError(ex.Message, 0, ex.ToString()); }
            _free_thread = null;
        }

    }
}
