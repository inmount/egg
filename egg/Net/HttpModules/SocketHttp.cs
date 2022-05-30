using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace egg.Net.HttpModules {

    /// <summary>
    /// 使用Socket重构的Http协议控制器
    /// </summary>
    public class SocketHttp : egg.BasicObject {

        /// <summary>
        /// 获取提交管理器
        /// </summary>
        public HttpRequest Request { get; private set; }

        /// <summary>
        /// 获取接收管理器
        /// </summary>
        public HttpResponse Response { get; protected set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="url"></param>
        public SocketHttp(string url) {
            //rnd = new Random();
            this.Request = new HttpRequest(url);
            //this.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) dpz/1.0";

        }

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <returns></returns>
        public int Get() {

            bool isHead = true;
            bool isBody = false;
            //List<byte> head = new List<byte>();
            List<byte> bodyLen = new List<byte>();
            byte[] body = null;
            int bodyIndex = 0;

            string ip = eggs.Net.GetIPv4Address(Request.Uri.Host);
            int port = Request.Uri.Port;
            if (port <= 0) port = 443;
            string path = (Request.Uri.Path == "" ? "/" : Request.Uri.Path) + Request.Uri.QueryString;

            //接收解析
            string repProtocol = "";
            int repStatus = 0;
            HttpHeaders repHeader = new HttpHeaders();

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {

                //连接服务器
                socket.Connect(ip, port);
                //Thread.Sleep(10);    //等待10毫秒 

                //设置ssl
                NetworkStream ssl = new NetworkStream(socket);
                //SslStream ssl = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                //ssl.AuthenticateAsClient(Request.Uri.Host);

                //发送HTTP头部信息
                byte[] buffer = Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n");
                ssl.Write(buffer, 0, buffer.Length);
                //socket.Send(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));

                foreach (var item in Request.Header) {
                    //socket.Send(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
                    buffer = Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n");
                    ssl.Write(buffer, 0, buffer.Length);
                }
                //socket.Send(Encoding.ASCII.GetBytes($"\r\n"));
                buffer = Encoding.ASCII.GetBytes($"\r\n");
                ssl.Write(buffer, 0, buffer.Length);
                ssl.Flush();

                //接收内容处理变量
                //int size = 0;
                bool isEnd = false;
                int line = 0;
                bool lr = false;
                int bufferSize = 4096;

                //解析头部信息专用变量
                bool isKey = true;
                string keyStr = "";
                string valueStr = "";
                //Span<byte> span = new Span<byte>();

                while (!isEnd) {
                    //size = ssl.Read(span);
                    if (isBody) {
                        //当为内容时直接将内容添加到对象中
                        if (body.Length - bodyIndex < bufferSize) bufferSize = body.Length - bodyIndex;
                        int size = ssl.Read(body, bodyIndex, bufferSize);

                        //增加索引并判断结尾
                        if (size > 0) {
                            bodyIndex += size;
                            if (bodyIndex >= body.Length) isEnd = true;
                        } else {
                            isEnd = true;
                        }
                    } else if (isHead) {
                        //当为头部时则进行头部处理
                        int bs = ssl.ReadByte();
                        if (bs > 0) {
                            switch (bs) {
                                case 13://回车
                                    lr = true;
                                    //dpz.Debug.WriteLine($"[{i}]Find '\\r',rnCount:{rnCount}");
                                    break;
                                case 10://换行
                                    if (lr) {
                                        lr = false;//重置回车标志
                                        line++;
                                        if (line == 2) {
                                            //头部定义结束
                                            isHead = false;
                                        } else {
                                            //协议为空则为第一行，优先解析协议
                                            if (repProtocol.IsEmpty()) {
                                                string[] headStatuses = keyStr.Split(' ');
                                                if (headStatuses.Length < 2) throw new Exception("头部信息无效");
                                                repProtocol = headStatuses[0];
                                                repStatus = headStatuses[1].ToInteger();
                                            } else {
                                                //添加头部信息
                                                repHeader[keyStr.Trim()] = valueStr.Trim();
                                            }
                                        }
                                    }
                                    break;
                                case 91://冒号
                                    if (isKey) {
                                        isKey = false;
                                    } else {
                                        valueStr += ':';
                                    }
                                    break;
                                default:
                                    char chr = (char)bs;
                                    if (isKey) { keyStr += chr; } else { valueStr += chr; }
                                    line = 0;
                                    lr = false;
                                    //head.Add((byte)bs);
                                    break;
                            }
                        } else {
                            isEnd = true;
                        }
                    } else {
                        //当为结束了头部又未开始主体内容时则为内容长度定义处理
                        int bs = ssl.ReadByte();
                        if (bs > 0) {
                            switch (bs) {
                                case 13://回车
                                    lr = true;
                                    //dpz.Debug.WriteLine($"[{i}]Find '\\r',rnCount:{rnCount}");
                                    break;
                                case 10://换行
                                    if (lr) {
                                        //获取主体长度并定义主体
                                        string lenStr = System.Text.Encoding.ASCII.GetString(bodyLen.ToArray());
                                        int len = Convert.ToInt32(lenStr, 16);
                                        egg.Debug.WriteLine($" [+]Find '\\r',rnCount:{len}");
                                        body = new byte[len];
                                        bodyIndex = 0;
                                        isBody = true;
                                    }
                                    break;
                                default:
                                    lr = false;
                                    bodyLen.Add((byte)bs);
                                    break;
                            }
                        } else {
                            isEnd = true;
                        }

                    }
                }
            }

            this.Response = new HttpResponse(repProtocol, repStatus, repHeader, body);

            return repStatus;

        }

    }
}
