using System.Net;

namespace egg.Mvc {

    /// <summary>
    /// Kestrel操作器
    /// </summary>
    public class KestrelConfig :egg.File.ConfFile {

        private egg.File.Conf.SettingGroup _server;
        private egg.File.Conf.SettingGroup _http;
        private egg.File.Conf.SettingGroup _https;

        private bool CheckEnable(string config) {
            if (config.IsNoneOrNull()) return false;
            config = config.ToLower();
            return (config == "yes" || config == "true");
        }

        /// <summary>
        /// 获取或设置服务有效性
        /// </summary>
        public bool Enable { get { return CheckEnable(_server["Enable"]); } set { _server["Enable"] = value ? "yes" : "no"; } }

        /// <summary>
        /// 获取或设置HTTP服务有效性
        /// </summary>
        public bool HttpEnable { get { return CheckEnable(_http["Enable"]); } set { _http["Enable"] = value ? "yes" : "no"; } }

        /// <summary>
        /// 获取或设置HTTPS服务有效性
        /// </summary>
        public bool HttpsEnable { get { return CheckEnable(_https["Enable"]); } set { _https["Enable"] = value ? "yes" : "no"; } }

        /// <summary>
        /// 获取或设置HTTP服务端口
        /// </summary>
        public int HttpPort { get { return _http["Port"].ToInteger(); } set { _http["Port"] = $"{value}"; } }

        /// <summary>
        /// 获取或设置HTTPS服务端口
        /// </summary>
        public int HttpsPort { get { return _https["Port"].ToInteger(); } set { _https["Port"] = $"{value}"; } }

        /// <summary>
        /// 获取或设置HTTPS证书路径
        /// </summary>
        public string HttpsPfxPath { get { return _https["Pfx.Path"]; } set { _https["Pfx.Path"] = value; } }

        /// <summary>
        /// 获取或设置HTTPS证书密码
        /// </summary>
        public string HttpsPfxPwd { get { return _https["Pfx.Password"]; } set { _https["Pfx.Password"] = value; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        public KestrelConfig(string path) : base(path) {

            bool needSave = false;

            _server = base["Server"];
            if (!_server.ContainsKey("Enable")) {
                this.Enable = false;
                needSave = true;
            }

            // 初始化HTTP配置
            _http = base["HTTP"];
            if (!_http.ContainsKey("Enable")) {
                this.HttpEnable = false;
                needSave = true;
            }
            if (!_http.ContainsKey("Port")) {
                this.HttpPort = 80;
                needSave = true;
            }

            // 初始化HTTPS配置
            _https = base["HTTPS"];
            if (!_https.ContainsKey("Enable")) {
                this.HttpsEnable = false;
                needSave = true;
            }
            if (!_https.ContainsKey("Port")) {
                this.HttpsPort = 8080;
                needSave = true;
            }
            if (!_https.ContainsKey("Pfx.Path")) {
                this.HttpsPfxPath = "/ssl/ssl.pfx";
                needSave = true;
            }
            if (!_https.ContainsKey("Pfx.Password")) {
                this.HttpsPfxPwd = "123456";
                needSave = true;
            }

            // 需要保存
            if (needSave) base.Save();

        }

    }
}
