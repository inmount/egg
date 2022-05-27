using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net {

    /// <summary>
    /// DNS专用类
    /// </summary>
    public class Dns {

        /// <summary>
        /// 将域名解析为对应的IP地址
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetIPv4Address(string host) {
            if (host.IsIPv4()) return host;

            System.Net.IPAddress[] iPs = System.Net.Dns.GetHostEntry(host).AddressList;
            for (int i = 0; i < iPs.Length; i++) {
                string ip = iPs[i].ToString();
                if (ip.IsIPv4())
                    return ip;
            }

            throw new Exception("解析失败");

        }
    }
}
