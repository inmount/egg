using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Mvc {

    /// <summary>
    /// 支持ssr.SessionServer的WebApi基础类
    /// </summary>
    public abstract class ApiControllerSessionBase : ApiControllerBase {

        /// <summary>
        /// 获取交互信息管理器
        /// </summary>
        public ISessionManager Session { get; private set; }

        /// <summary>
        /// 设置交互信息管理器
        /// </summary>
        /// <param name="sessionManager"></param>
        protected void SetSessionManager(ISessionManager sessionManager) {
            this.Session = sessionManager;
        }

    }
}
