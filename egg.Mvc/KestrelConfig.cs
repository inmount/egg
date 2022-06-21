using System;
using System.Collections.Generic;
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
        public List<KestrelHttpConfig> HttpConfigs { get; private set; }

        /// <summary>
        /// 获取或设置服务有效性
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        public KestrelConfig(string path) {
            // 新建配置集合
            this.HttpConfigs = new List<KestrelHttpConfig>();
            // 建立默认配置文件
            if (!eggs.IO.CheckFileExists(path)) eggs.Mvc.CreateDefaultKestrelConfigFile(path);
            try {
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
                            // 建立配置管理器
                            KestrelHttpConfig config = new KestrelHttpConfig();
                            this.HttpConfigs.Add(config);
                            config.Enable = CheckEnable(http["Enable"]);
                            config.IPAddress = http["IP"];
                            config.Port = http["Port"].ToInteger();
                            // 读取所有证书设置
                            if (!http["Certs"].IsEmpty()) {
                                string[] certs = http["Certs"].Split(';');
                                for (int j = 0; j < certs.Length; j++) {
                                    if (!certs[j].IsEmpty()) {
                                        // 新建证书管理器
                                        KestrelCertConfig certConfig = new KestrelCertConfig();
                                        config.Certs.Add(certConfig);
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

            } catch (Exception ex) {
                throw new System.Exception("读取配置发生异常", ex);
            }
        }

    }
}
