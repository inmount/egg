using System.Net;

namespace egg.Mvc {

    /// <summary>
    /// Kestrel操作器
    /// </summary>
    public class KestrelConfig : egg.BasicObject {

        // 检测是否可用
        private bool CheckEnable(string config) {
            if (config.IsEmpty()) return false;
            config = config.ToLower();
            return (config == "yes" || config == "true");
        }

        /// <summary>
        /// 获取协议配置集合
        /// </summary>
        public KeyList<KestrelHttpConfig> HttpConfigs { get; private set; }

        /// <summary>
        /// 获取或设置服务有效性
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        public KestrelConfig(string path) {

            if (!eggs.IO.CheckFileExists(path)) eggs.Mvc.CreateDefaultKestrelConfigFile(path);

            using (var f = eggs.IO.OpenConfigDocument(path)) {
                var doc = f.Document;
                // 设定服务组
                var server = doc["server"];
                this.Enable = CheckEnable(server["Enable"]);
                // 读取所有设置项
                string[] configs = server["Configs"].Split(';');
                for (int i = 0; i < configs.Length; i++) {
                    if (!configs[i].IsEmpty()) {
                        // HTTP组
                        var http = doc[configs[i]];
                        KestrelHttpConfig config = new KestrelHttpConfig();
                        config.Enable = CheckEnable(http["Enable"]);
                        config.IPAddress = http["IP"];
                        config.Port = http["Port"].ToInteger();
                        // 读取所有证书设置
                        if (!http["Certs"].IsEmpty()) {
                            string[] certs = http["Certs"].Split(';');
                            for (int j = 0; j < certs.Length; j++) {
                                if (!certs[j].IsEmpty()) {
                                    KestrelCertConfig certConfig = new KestrelCertConfig();
                                    // cert组
                                    var cert = doc[certs[j]];
                                    certConfig.Enable = CheckEnable(cert["Enable"]);
                                    certConfig.Path = cert["Path"];
                                    certConfig.Password = cert["Password"];
                                }
                            }
                        }
                    }
                }

                f.Save();
            }

        }

    }
}
