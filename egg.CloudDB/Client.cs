using System;
using egg;
using Json = egg.Serializable.Json;

namespace egg.CloudDB {

    /// <summary>
    /// 通讯客户端
    /// </summary>
    public class Client : egg.BasicObject {

        /// <summary>
        /// 获取服务器设置
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// 获取交互令牌
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="server"></param>
        public Client(string server) {
            this.Server = server;
            this.Token = "";
        }

        // 获取签名
        private string GetSign(string gid, string ts, string key) {
            return $"Gid={gid}&Timestamp={ts}&key={key}".GetMD5();
        }

        /// <summary>
        /// 获取应答器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public egg.Jttp.JttpResponse GetResponse(string url, Serializable.Json.Object data = null) {
            try {
                using (egg.Jttp.JttpRequest request = new Jttp.JttpRequest()) {
                    request.Timestamp = "" + eggs.Time.GetNow().ToTimeStamp();
                    request.Token = this.Token;
                    if (data.IsNull()) {
                        request.Form = new Serializable.Json.Object();
                    } else {
                        request.Form = data;
                    }
                    return egg.Jttp.JttpClient.Post(this.Server + url, request);
                }
            } catch (Exception ex) {
                throw new Exception($"获取 Response 发生异常：[url:{url}]", ex);
            }
        }

        /// <summary>
        /// 获取应答器
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public egg.Jttp.JttpResponse GetResponse(string gid, string key, string url, Serializable.Json.Object data = null) {
            using (egg.Jttp.JttpRequest request = new Jttp.JttpRequest()) {
                request.Timestamp = "" + eggs.Time.GetNow().ToTimeStamp();
                request.Token = this.Token;
                request.Signature = GetSign(gid, request.Timestamp, key);
                if (data.IsNull()) {
                    request.Form = new Serializable.Json.Object();
                } else {
                    request.Form = data;
                }
                return egg.Jttp.JttpClient.Post(this.Server + url, request);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string name, string password) {
            Json.Object data = new Json.Object();
            // 设置参数
            data["Name"] = name;
            data["Pwd"] = password;
            using (egg.Jttp.JttpResponse response = GetResponse("/User/Login", data)) {
                if (response.Result > 0) {
                    this.Token = response.Token;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetToken(string gid, string key) {
            Json.Object data = new Json.Object();
            // 获取时间
            string dt = "" + eggs.Time.GetNow().ToTimeStamp();
            // 设置参数
            data["Gid"] = gid;
            data["GidSign"] = GetSign(gid, dt, key);
            // 建立提交器
            using (egg.Jttp.JttpRequest request = new Jttp.JttpRequest()) {
                request.Timestamp = dt;
                request.Token = this.Token;
                if (data.IsNull()) {
                    request.Form = new Serializable.Json.Object();
                } else {
                    request.Form = data;
                }
                using (egg.Jttp.JttpResponse response = egg.Jttp.JttpClient.Post(this.Server + "/User/Token", request)) {
                    if (response.Result > 0) {
                        this.Token = response.Token;
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 保持令牌
        /// </summary>
        /// <returns></returns>
        public bool KeepToken() {
            using (egg.Jttp.JttpResponse response = GetResponse("/User/TokenKeep")) {
                return response.Result > 0;
            }
        }

        /// <summary>
        /// 使用用户名创建一个客户端
        /// </summary>
        /// <param name="server"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Client CreateByUser(string server, string name, string password) {
            Client client = new Client(server);
            if (client.Login(name, password)) return client;
            return null;
        }

        /// <summary>
        /// 使用Gid创建一个客户端
        /// </summary>
        /// <param name="server"></param>
        /// <param name="gid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Client CreateByGid(string server, string gid, string key) {
            Client client = new Client(server);
            if (client.GetToken(gid, key)) return client;
            return null;
        }
    }
}
