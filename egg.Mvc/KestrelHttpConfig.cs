using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Mvc {

    /// <summary>
    /// Pfx证书文件信息
    /// </summary>
    public class KestrelHttpConfig : egg.BasicObject {

        /// <summary>
        /// 获取或设置可用性
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 开启端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取证书集合
        /// </summary>
        public List<KestrelCertConfig> Certs { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public KestrelHttpConfig() {
            this.Certs = new List<KestrelCertConfig>();
        }

    }
}
