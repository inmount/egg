using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace egg.Mvc {

    /// <summary>
    /// Kestrel操作器
    /// </summary>
    public static class Kestrel {

        private static bool CheckEnable(string config) {
            if (config.IsNoneOrNull()) return false;
            config = config.ToLower();
            return (config == "yes" || config == "true");
        }

        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="webBuilder"></param>
        public static void DeployConfig(IWebHostBuilder webBuilder, string path) {

            // 读取配置
            KestrelConfig config = new KestrelConfig(path);
            DeployConfig(webBuilder, config);

        }

        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="webBuilder"></param>
        /// <param name="config"></param>
        public static void DeployConfig(IWebHostBuilder webBuilder, KestrelConfig config) {

            // 判断是否启用Kestrel服务
            if (config.Enable) {
                webBuilder.ConfigureKestrel(options => {

                    // 判断是否启用HTTP配置
                    if (config.HttpEnable) {
                        // 填入配置中的监听端口
                        options.Listen(IPAddress.Any, config.HttpPort);
                    }

                    // 判断是否启用HTTPS配置
                    if (config.HttpsEnable) {
                        // 填入配置中的监听端口
                        options.Listen(IPAddress.Any, config.HttpsPort, listenOptions => {
                            // 填入配置中的pfx文件路径和指定的密码
                            listenOptions.UseHttps(config.HttpsPfxPath, config.HttpsPfxPwd);
                        });
                    }

                });
            }
        }

    }
}
