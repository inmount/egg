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
    /// 使用Socket重构的Https协议控制器
    /// </summary>
    public class SocketHttps : SocketHttp {

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="url"></param>
        public SocketHttps(string url) : base(url) { }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            ////此处报错 根据验证过程，远程证书无效。也就是这个地方了
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //    return true;
            //Console.WriteLine("Certificate error证书错误: {0}", sslPolicyErrors);
            //return false;
            return true;
        }

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <returns></returns>
        public new int Get() {

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
                NetworkStream networkStream = new NetworkStream(socket);
                SslStream ssl = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                ssl.AuthenticateAsClient(Request.Uri.Host);

                //发送HTTP头部信息
                ssl.Write(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));
                //socket.Send(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));

                foreach (var item in Request.Header) {
                    //socket.Send(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
                    ssl.Write(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
                }
                //socket.Send(Encoding.ASCII.GetBytes($"\r\n"));
                ssl.Write(Encoding.ASCII.GetBytes($"\r\n"));
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
                                        eggs.Debug.WriteLine($" [+]Find '\\r',rnCount:{len}");
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

            base.Response = new HttpResponse(repProtocol, repStatus, repHeader, body);

            return repStatus;

        }

        ///// <summary>
        ///// 以Get方式获取数据
        ///// </summary>
        ///// <returns></returns>
        //public new int Get() {

        //    byte[] bsHead = new byte[0];
        //    byte[] bsBody = new byte[0];

        //    //using (dpz.Data.Url urls = new Data.Url(url)) {
        //    string ip = dpz.Net.Dns.GetIPv4Address(Request.Uri.Host);
        //    int port = Request.Uri.Port.ToInteger();
        //    if (port <= 0) port = 443;
        //    string path = (Request.Uri.Path == "" ? "/" : Request.Uri.Path) + Request.Uri.QueryString;

        //    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {

        //        //连接服务器
        //        socket.Connect(ip, port);

        //        Thread.Sleep(100);    //等待100毫秒 

        //        NetworkStream networkStream = new NetworkStream(socket);
        //        SslStream ssl = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
        //        ssl.AuthenticateAsClient(Request.Uri.Host);

        //        //发送HTTP头部信息
        //        ssl.Write(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));
        //        //socket.Send(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));

        //        foreach (var item in Request.Header) {
        //            //socket.Send(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
        //            ssl.Write(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
        //        }
        //        //socket.Send(Encoding.ASCII.GetBytes($"\r\n"));
        //        ssl.Write(Encoding.ASCII.GetBytes($"\r\n"));
        //        ssl.Flush();

        //        int size = 0;
        //        byte[] buffer = new byte[4096];
        //        bool isBody = false;
        //        int rnCount = 0;
        //        bool isR = false;
        //        do {
        //            size = ssl.Read(buffer, 0, buffer.Length);
        //            if (size > 0) {
        //                //当非第一次存储时，先扩张一次存储单元
        //                if (isBody) {

        //                    if (bsBody.Length > 0) {
        //                        byte[] tp = bsBody;
        //                        bsBody = new byte[tp.Length + size];
        //                        Array.Copy(tp, 0, bsBody, 0, tp.Length);
        //                        Array.Copy(buffer, 0, bsBody, tp.Length, size);
        //                    } else {
        //                        bsBody = new byte[size];
        //                        Array.Copy(buffer, 0, bsBody, 0, size);
        //                    }

        //                } else {

        //                    //检测头部是否结束
        //                    for (int i = 0; i < size; i++) {
        //                        switch (buffer[i]) {
        //                            case 13://回车
        //                                isR = true;
        //                                //dpz.Debug.WriteLine($"[{i}]Find '\\r',rnCount:{rnCount}");
        //                                break;
        //                            case 10://换行
        //                                    //dpz.Debug.WriteLine($"[{i}]Find '\\n',r:{isR}");
        //                                if (isR) {
        //                                    rnCount++;
        //                                    //dpz.Debug.WriteLine($"[{i}]rnCount:{rnCount}");
        //                                    //当有两个回车换行时，头部定义结束
        //                                    if (rnCount == 2) {

        //                                        int sizeHead = i + 1;
        //                                        int sizeBody = size - sizeHead;
        //                                        size = sizeHead;
        //                                        //当本次数据有多余时，填充至本体数据
        //                                        if (sizeBody > 0) {
        //                                            bsBody = new byte[sizeBody];
        //                                            Array.Copy(buffer, sizeHead, bsBody, 0, sizeBody);
        //                                        }
        //                                        isBody = true;//切换数据接受至本体
        //                                    }
        //                                }
        //                                isR = false;
        //                                break;
        //                            default:
        //                                isR = false;
        //                                rnCount = 0;
        //                                break;
        //                        }
        //                    }

        //                    //接收头部数据
        //                    if (bsHead.Length > 0) {
        //                        byte[] tp = bsHead;
        //                        bsHead = new byte[tp.Length + size];
        //                        Array.Copy(tp, 0, bsHead, 0, tp.Length);
        //                        Array.Copy(buffer, 0, bsHead, tp.Length, size);
        //                    } else {
        //                        bsHead = new byte[size];
        //                        Array.Copy(buffer, 0, bsHead, 0, size);
        //                    }

        //                    //头部数据接收结束，本体数据接收开始
        //                    if (isBody) {
        //                        //string headString = Encoding.ASCII.GetString(bsHead);
        //                        //this.ResponseHeaderString = headString;
        //                    }
        //                }
        //            }
        //        } while (size > 0);


        //        if (socket.Connected) socket.Close();
        //    }

        //    //获取内容长度
        //    string hLen = "";
        //    int bodyLen = 0;
        //    for (int i = 0; i < bsBody.Length; i++) {
        //        switch (bsBody[i]) {
        //            case 10://检测到换行，长度定义结束
        //                bodyLen = Convert.ToInt32(hLen, 16);
        //                byte[] tp = bsBody;
        //                bsBody = new byte[bodyLen];
        //                Array.Copy(tp, i + 1, bsBody, 0, bodyLen);
        //                break;
        //            case 13: break;
        //            default:
        //                hLen += (char)bsBody[i];
        //                break;
        //        }
        //        if (bodyLen > 0) break;
        //    }

        //    base.Response = new HttpResponse(bsHead, bsBody);

        //    return Response.Status;
        //}

    }
}
