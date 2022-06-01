using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Mvc {

    /// <summary>
    /// 证书文件信息
    /// </summary>
    public class KestrelCertConfig {

        /// <summary>
        /// 获取或设置可用性
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }
}
