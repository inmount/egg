using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace egg.Mvc {

    /// <summary>
    /// Kestrel操作器
    /// </summary>
    public static class Kestrel {

        private const string Log_Object_Name = "egg.Mvc.Kestrel";

        private static bool CheckEnable(string config) {
            if (config.IsEmpty()) return false;
            config = config.ToLower();
            return (config == "yes" || config == "true");
        }

        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="webBuilder"></param>
        /// <param name="logger"></param>
        public static void DeployConfig(IWebHostBuilder webBuilder, string path, egg.SqliteLog.Logger logger = null) {
            // 读取配置
            KestrelConfig config = new KestrelConfig(path);
            // 输出日志
            if (!logger.IsNull()) logger.LogInfo(Log_Object_Name, "DeployConfig", $"path:{path}");
            // 应用配置
            DeployConfig(webBuilder, config, logger);

        }

        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="webBuilder"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public static void DeployConfig(IWebHostBuilder webBuilder, KestrelConfig config, egg.SqliteLog.Logger logger = null) {

            // 判断是否启用Kestrel服务
            if (config.Enable) {
                webBuilder.ConfigureKestrel(options => {
                    // 不使用
                    options.AddServerHeader = false;
                    // 输出日志
                    if (!logger.IsNull()) logger.LogInfo(Log_Object_Name, "DeployConfig", $"config.HttpConfigs:[{config.HttpConfigs.Count}]");
                    // 读取所有的配置信息
                    foreach (var http in config.HttpConfigs) {
                        if (http.Enable) {
                            // 判断是否包含证书
                            if (http.Certs.Count > 0) {
                                // 输出日志
                                if (!logger.IsNull()) logger.LogInfo(Log_Object_Name, "DeployConfig", $"https://{http.IPAddress}:{http.Port}");
                                // https
                                options.Listen(http.IPAddress == "*" ? IPAddress.Any : IPAddress.Parse(http.IPAddress), http.Port, listenOptions => {
                                    // 填入配置中的pfx文件路径和指定的密码
                                    foreach (var cert in http.Certs) {
                                        if (cert.Enable) {
                                            if (!logger.IsNull()) logger.LogInfo(Log_Object_Name, "DeployConfig", $"cert:{cert.Path},{cert.Password}");
                                            listenOptions.UseHttps(cert.Path, cert.Password);
                                        }
                                    }
                                });
                            } else {
                                // 输出日志
                                if (!logger.IsNull()) logger.LogInfo(Log_Object_Name, "DeployConfig", $"http://{http.IPAddress}:{http.Port}");
                                // http
                                options.Listen(http.IPAddress == "*" ? IPAddress.Any : IPAddress.Parse(http.IPAddress), http.Port);
                            }
                        }
                    }

                    //// 判断是否启用HTTP配置
                    //if (config.HttpEnable) {
                    //    // 填入配置中的监听端口
                    //    options.Listen(IPAddress.Any, config.HttpPort);
                    //}

                    //// 判断是否启用HTTPS配置
                    //if (config.HttpsEnable) {
                    //    // 填入配置中的监听端口
                    //    options.Listen(IPAddress.Any, config.HttpsPort, listenOptions => {
                    //        // 填入配置中的pfx文件路径和指定的密码
                    //        listenOptions.UseHttps(config.HttpsPfxPath, config.HttpsPfxPwd);
                    //    });
                    //}

                });
            }
        }

    }
}
