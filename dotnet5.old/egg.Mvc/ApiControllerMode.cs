using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Mvc {

    /// <summary>
    /// Api控制器工作模式
    /// </summary>
    public enum ApiControllerMode {

        /// <summary>
        /// 原生模式
        /// </summary>
        Native=0x00,

        /// <summary>
        /// 表单模式
        /// </summary>
        Form=0x01,

        /// <summary>
        /// Jttp协议模式
        /// </summary>
        Jttp=0x02

    }
}
