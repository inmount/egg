using System;
using System.Collections.Generic;
using System.Text;

namespace egg {
    /// <summary>
    /// 基本实体
    /// </summary>
    public class BasicObject : Basic {

        /// <summary>
        /// 获取BasicObject唯一标识符
        /// </summary>
        public long BoId { internal set; get; }

        /// <summary>
        /// 获取BasicObject管理器
        /// </summary>
        public BasicObjectsMnanger BoManager { internal set; get; }

        /// <summary>
        /// 获取BasicObject拥有者
        /// </summary>
        public Object BoOwner {
            get {
                if (eggs.Object.IsNull(this.BoManager)) return null;
                return this.BoManager.Owner;
            }
        }

    }
}
